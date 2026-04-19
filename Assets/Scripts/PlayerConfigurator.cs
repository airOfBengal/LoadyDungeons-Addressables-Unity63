using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.XR;

// Used for the Hat selection logic
public class PlayerConfigurator : MonoBehaviour
{
    [SerializeField]
    private Transform m_HatAnchor;

    private GameObject m_HatInstance;

    private AsyncOperationHandle<GameObject> m_HatLoadOpHandle;

    void Start()
    {           
        LoadInRandom();
    }

    private void Update() {
        if(Input.GetMouseButtonUp(1))
        {
            Destroy(m_HatInstance);
            Addressables.ReleaseInstance(m_HatLoadOpHandle);

            LoadInRandom();
        }
    }

    private void LoadInRandom()
    {
        int randomIndex = UnityEngine.Random.Range(0, 6);
        string hatAddress = string.Format("Hat{0:00}", randomIndex);

        m_HatLoadOpHandle = Addressables.LoadAssetAsync<GameObject>(hatAddress);
        m_HatLoadOpHandle.Completed += OnHatLoadComplete;
    }

    private void OnHatLoadComplete(AsyncOperationHandle<GameObject> handle)
    {
        if(handle.Status == AsyncOperationStatus.Succeeded)
        {
            m_HatInstance = Instantiate(handle.Result, m_HatAnchor);
        }
    }

    private void OnDisable()
    {
        m_HatLoadOpHandle.Completed -= OnHatLoadComplete;
    }
}
