using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
public class BallController : MonoBehaviour
{
    public static BallController Singleton { get; private set; }
    private Collider mainCollider;
    private int timeForEffect;
    [SerializeField] private GameObject effectTimeViewGameObject, LeftMovePanel, RightMovePanel;
    [SerializeField] private TextMeshProUGUI effectTimeView;
    [SerializeField] private Texture[] skins = new Texture[2];
    private Material LeftWall, RightWall;
    [SerializeField] private AudioSource DestroyCubeSound;
    [SerializeField] private MeshRenderer LeftWallRdr, RightWallRdr;
    private MeshRenderer mainMeshRenderer;
    private Rigidbody body;
    private Transform currentTransform;
    private ValueManager valueManager;
    private CubeSpawner cubeSpawner;
    private int posIndex = 0;
    private bool isMoving = false;
    private Game game;
    private Dictionary<int, float[]> raceXPos = new Dictionary<int, float[]>()
    {
        [2] = new float[] { 0.5f, -0.5f },
        [3] = new float[] { 0.6f, 0f, -0.6f },
        [4] = new float[] { 0.67f, 0.25f, -0.25f, -0.67f }
    };
    public bool IsInEffect
    {
        get
        {
            return mainMeshRenderer.material.color.a == 0.25f;
        }
    }
    public bool IsTimeTextActive
    {
        get
        {
            return effectTimeViewGameObject.activeSelf;
        }
    }
    private bool CanMove
    {
        get
        {
            string leftTag = "", rightTag = "";
            if (Physics.Raycast(body.position, Vector3.left, out RaycastHit leftHit, 1f))
            {
                leftTag = leftHit.collider.tag;
            }
            if (Physics.Raycast(body.position, Vector3.right, out RaycastHit rightHit, 1f))
            {
                rightTag = rightHit.collider.tag;
            }
            return leftTag != "Cube" && rightTag != "Cube";
        }
    }
    private void Awake()
    {
        Singleton = this;
    }
    private IEnumerator Start()
    {
        var skinIndex = Preferences.EquipedSkin;
        mainMeshRenderer = GetComponent<MeshRenderer>();
        mainCollider = GetComponent<Collider>();
        if (skinIndex != -1)
        {
            mainMeshRenderer.material.mainTexture = skins[skinIndex];
        }
        game = Game.Singleton;
        valueManager = ValueManager.Singleton;
        cubeSpawner = CubeSpawner.Singleton;
        body = GetComponent<Rigidbody>();
        currentTransform = transform;
        LeftWall = LeftWallRdr.material;
        RightWall = RightWallRdr.material;
        valueManager.CubeCountChanged += delegate
        {
            IEnumerator localMove()
            {
                yield return new WaitForSeconds(0.3f);
                Move(0f, false);
            }
            StartCoroutine(localMove());
        };
        var tutorialManager = TutorialManager.shared;
        if (!tutorialManager.TutorialCompleted)
            yield return new WaitUntil(() => tutorialManager.TutorialCompleted);
        LeftMovePanel.SetActive(true);
        RightMovePanel.SetActive(true);
        effectTimeView.gameObject.SetActive(true);
        DisplayTargetColor();
        StartCoroutine(timeEstimated());
    }
    private IEnumerator timeEstimated()
    {
        var delay = new WaitForSeconds(1f);
        for (timeForEffect = 31; timeForEffect > 0; timeForEffect--)
        {
            effectTimeView.text = timeForEffect.ToString();
            yield return delay;
        }
        effectTimeView.text = LocalizeManager.GetLocalizedString(LocalizeManager.TapHereToGoThroughAnyCube, true);
    }
    private void Move(float x, bool changePosIndex = true)
    {
        var currentXPos = raceXPos[valueManager.GetCubeCount(false)];
        if (changePosIndex)
            posIndex = Mathf.Clamp(posIndex + (x < 0f ? -1 : 1), 0, currentXPos.Length - 1);
        var target = new Vector3(currentXPos[posIndex], 0.175f, 3.01f);
        IEnumerator Moving()
        {
            isMoving = true;
            var lerp = 0f;
            for (int i = 0; i < 10; i++)
            {
                lerp += 0.1f;
                body.MovePosition(Vector3.Lerp(body.position, target, lerp));
                yield return new WaitForFixedUpdate();
            }
            isMoving = false;
        }
        if (isMoving)
            StopCoroutine(Moving());
        StartCoroutine(Moving());
    }
    public void SetTimeTextActive(bool active)
    {
        effectTimeViewGameObject.SetActive(active);
    }
    public void StartInvisibleAndTriigerEffect()
    {
        if (timeForEffect == 0 && game.State == GameState.Running)
        {
            SetTimeTextActive(false);
            mainMeshRenderer.material.color = new Color(1f, 1f, 1f, 0.25f);
            mainCollider.isTrigger = true;
        }
    }
    private void DisplayTargetColor()
    {
        var color = valueManager.TargetColor;
        LeftWall.color = color;
        RightWall.color = color;
    }
    private void FixedUpdate()
    {
        currentTransform.Rotate(50f * valueManager.GetCubeSpeed(false) * Vector3.left);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cube"))
        {
            SpawnCubes();
        }
    }
    private void SpawnCubes()
    {
        cubeSpawner.UpdateColor();
        DisplayTargetColor();
        cubeSpawner.Spawn();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Cube"))
        {
            mainMeshRenderer.material.color = new Color(1f, 1f, 1f, 1f);
            mainCollider.isTrigger = false;
            SetTimeTextActive(true);
            StartCoroutine(timeEstimated());
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        var gameObject = collision.gameObject;
        var name = gameObject.name;
        if (gameObject.CompareTag("Cube"))
        {
            if (name.Contains("targetColor"))
            {
                DestroyCubeSound.Play();
                Destroy(gameObject);
                SpawnCubes();
                valueManager.LastDestroyCaller = this;
                game.IncreaseDestroyedCubesCount(true);
            }
            else
            {
                game.GameOver();
            }
        }


    }
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            StartInvisibleAndTriigerEffect();
    }
#endif
    public void LeftMove()
    {
        if (game.State == GameState.Running && CanMove)
            Move(-1f);
    }
    public void RightMove()
    {
        if (game.State == GameState.Running && CanMove)
            Move(1f);
    }
}
