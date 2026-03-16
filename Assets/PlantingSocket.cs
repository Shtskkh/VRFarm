using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PlantingSocket : MonoBehaviour
{
    [Header("Prefabs")]
    [Tooltip("Префаб рассады, которая появляется после посадки")]
    public GameObject seedlingPrefab;

    [Tooltip("Префаб нового овоща, который появляется через 30 секунд")]
    public GameObject vegetablePrefab;

    [Header("Spawn Settings")]
    [Tooltip("Смещение по Y для появления рассады на верху блока")]
    public float seedlingHeightOffset = 0.5f;

    [Tooltip("Позиция и смещение для нового овоща")]
    public Vector3 vegetableSpawnOffset = new Vector3(0f, 1.5f, 0f);

    [Tooltip("Задержка перед появлением нового овоща (секунды)")]
    public float regrowTime = 30f;

    private XRSocketInteractor _socket;
    private GameObject _currentSeedling;
    private bool _isGrowing = false;

    void Awake()
    {
        _socket = GetComponent<XRSocketInteractor>();

        if (_socket == null)
        {
            Debug.LogError("[PlantingSocket] XRSocketInteractor не найден на объекте!");
            return;
        }

        _socket.selectEntered.AddListener(OnVegetablePlaced);
    }

    void OnDestroy()
    {
        if (_socket != null)
            _socket.selectEntered.RemoveListener(OnVegetablePlaced);
    }

    private void OnVegetablePlaced(SelectEnterEventArgs args)
    {
        if (_isGrowing) return;

        StartCoroutine(PlantingSequence(args.interactableObject as MonoBehaviour));
    }

    private IEnumerator PlantingSequence(MonoBehaviour vegetable)
    {
        _isGrowing = true;

        // Один кадр паузы, чтобы XR Toolkit завершил Select
        yield return null;

        // Уничтожаем овощ
        if (vegetable != null)
            Destroy(vegetable.gameObject);

        // Отключаем сокет, чтобы ничего лишнего не вставили
        _socket.socketActive = false;

        // Спавним рассаду на верху блока
        Vector3 seedlingPosition = transform.position + Vector3.up * seedlingHeightOffset;
        _currentSeedling = Instantiate(seedlingPrefab, seedlingPosition, Quaternion.Euler(0f, 180f, 0f));

        Debug.Log($"[PlantingSocket] Рассада посажена. Новый овощ через {regrowTime} сек.");

        // Ждём regrowTime
        yield return new WaitForSeconds(regrowTime);

        // Убираем рассаду
        if (_currentSeedling != null)
            Destroy(_currentSeedling);

        // Спавним новый овощ
        Vector3 vegetablePosition = transform.position + vegetableSpawnOffset;
        Instantiate(vegetablePrefab, vegetablePosition, Quaternion.identity);

        // Снова активируем сокет
        _socket.socketActive = true;
        _isGrowing = false;

        Debug.Log("[PlantingSocket] Новый овощ вырос!");
    }
}