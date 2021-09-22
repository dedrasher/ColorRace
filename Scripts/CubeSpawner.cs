using System.Collections.Generic;
using System.Collections;
using UnityEngine;
public class CubeSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject Cube;
    private int spawnCount = 0;
    private ValueManager valueManager;
    private int cubesCount = 2;
    public static CubeSpawner singleton { get; private set; }
    private Dictionary<int, Vector3> Scales = new Dictionary<int, Vector3> {
        [2] = new Vector3(45f, 45f, 45f),
        [3] = new Vector3(30f,30f,30f),
        [4] = new Vector3(22f,22f,22f)
    };
    private Dictionary<int, Vector3[]> SpawnPositons = new Dictionary<int, Vector3[]>
    {
        [2] = new Vector3[] {new Vector3(0.45f,0.45f, -12f), new Vector3(-0.45f, 0.45f, -12f)},
        [3] = new Vector3[] { new Vector3(0.6f, 0.3f, -12f), new Vector3(0f, 0.3f, -12f), new Vector3(-0.6f, 0.3f, -12f) },
        [4] = new Vector3[] { new Vector3(0.7f, 0.22f, -12f), new Vector3(0.2375f, 0.22f, -12f), new Vector3(-0.2375f,0.22f, -12f), new Vector3(-0.7f, 0.22f, -12f) }
    };
    private void Awake()
    {
        singleton = this;
    }
    private IEnumerator Start()
    {
        valueManager = ValueManager.singleton;
        var tutorialManager = TutorialManager.shared;
        if (!tutorialManager.TutorialCompleted)
            yield return new WaitUntil(() =>  tutorialManager.TutorialCompleted);
        UpdateColor(false);
        Spawn();
    }
    public void UpdateColor(bool init = true)
    {
        cubesCount = valueManager.GetCubeCount(spawnCount == 10 || spawnCount == 40);
        if(init)
        valueManager.InitColor();
    }
    public void Spawn()
    {   
            var targetIndex = Random.Range(0, cubesCount);
            var anotherColors = valueManager.AnotherColors;
            var spawnPositions = SpawnPositons[cubesCount];
            var scale = Scales[cubesCount];
            var speed = valueManager.GetCubeSpeed(true);      
          var cube = Instantiate(Cube, spawnPositions[targetIndex], Quaternion.identity);
            cube.name +=  " targetColor";
            cube.transform.localScale = scale;
            var cubeController = cube.GetComponent<CubeController>();
            cubeController.speed = speed;
            cubeController.SetColor(valueManager.TargetColor, true);
            for (int i = 0; i < anotherColors.Length; i++)
            {
                if (i != targetIndex)
                {
                    cube = Instantiate(Cube, spawnPositions[i], Quaternion.identity);
                    cube.transform.localScale = scale;
                    cubeController = cube.GetComponent<CubeController>();
                    cubeController.speed = speed;
                    cubeController.SetColor(anotherColors[i], false);
                }
            }
            spawnCount++;    
    }
}
