using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface IGameSceneController
{
    Task SceneMext();
    Task SceneReload();
}

public interface IManaged
{
    void Initialize();
    void Setup();
    IGameSceneController gameSceneController { get; set; }
}

internal class GameSceneController : MonoBehaviour, IGameSceneController
{
    [SerializeField]
    private int[] m_stageOrder;

    [SerializeField]
    private int m_finished;

    private int m_currentIndex = 0;

    public async Task SceneMext()
    {
        int nextIndex = m_currentIndex + 1;
        if (m_stageOrder.Length > nextIndex)
        {
            var currentSceneId = m_stageOrder[m_currentIndex];
            var nextSceneId = m_stageOrder[nextIndex];

            await Task.WhenAll(AsyncSceneManager.UnloadSceneAdditive(currentSceneId), AsyncSceneManager.LoadSceneAdditive(nextSceneId));
            OnLoad();
            m_currentIndex++;
        }
        else
        {
            await AsyncSceneManager.LoadScene(m_finished);
        }
    }

    public async Task SceneReload()
    {
        var currentSceneId = m_stageOrder[m_currentIndex];
        await Task.WhenAll(AsyncSceneManager.LoadSceneAdditive(currentSceneId), AsyncSceneManager.UnloadSceneAdditive(currentSceneId));

        OnLoad();
    }

    private void Start()
    {
        Initialize();
    }

    private async void Initialize()
    {
        await AsyncSceneManager.LoadSceneAdditive(m_stageOrder[m_currentIndex]);
        OnLoad();
    }

    private void OnLoad()
    {
        var manageds = FindObjectsOfType<MonoBehaviour>().OfType<IManaged>();
        foreach (var managed in manageds)
        {
            if (managed.gameSceneController != null) continue;
            managed.gameSceneController = this;
            managed.Initialize();
        }

        foreach (var managed in manageds)
        {
            managed.Setup();
        }
    }
}

internal static class AsyncSceneManager
{
    public static async Task LoadSceneAdditive(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId, LoadSceneMode.Additive);
        await TaskOperation(operation);
    }


    public static async Task LoadScene(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId, LoadSceneMode.Single);
        await TaskOperation(operation);
    }

    public static async Task UnloadSceneAdditive(int sceneId)
    {
        AsyncOperation operation = SceneManager.UnloadSceneAsync(sceneId);
        await TaskOperation(operation);
    }

    public static async Task TaskOperation(AsyncOperation operation)
    {
        var taskSource = new TaskCompletionSource<AsyncOperation>();

        operation.completed += taskSource.SetResult;
        await taskSource.Task;
    }
}
