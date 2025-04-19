using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Sirenix.OdinInspector;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Tilemaps;

//==============================================================================
// TileMapController
//==============================================================================
// Target의 포지션을 참조받아 해당 포지션에 따라 타일맵을 이동시키는 컨트롤러
// 해당 타일맵은 탑뷰 특히 뱀파이어 서바이벌에 해당하는 장르에서 무한맵을 구현하기 위한
// 컨트롤러이다.
//==============================================================================
public class TileMapController : MonoBehaviour, IMoveable
{
    [TabGroup("TileMap")][Tooltip("타일맵 사이즈(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private int _tileMapSize; // 40이 기본
    public float TileMapSize { get { return _tileMapSize / 100; } }

    [TabGroup("TileMap")][Tooltip("타일맵 오버랩 오프셋(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private int _tilemapOverlapOffset; // 0.1이 기본
    public float TileMapOverlapOffset { get { return _tilemapOverlapOffset / 100; } }

    [TabGroup("TileMap")][SerializeField] private List<GameObject> _tileMapList;
    public List<GameObject> TileMapList { get { return _tileMapList; } }

    [TabGroup("Target")][SerializeField] private GameObject _tileMapTarget;
    public GameObject TileMapTarget { get { return _tileMapTarget; } }

    [TabGroup("Target")][SerializeField] private IPositionProvider _tileMapTargetPosition;
    public IPositionProvider TileMapTargetPosition { get { return _tileMapTargetPosition; } }

    [TabGroup("Direction")][SerializeField] private Vector3 _moveDirection;
    public Vector3 MoveDirection { get { return _moveDirection; } set { _moveDirection = value; } }

    private bool CheckTileMap()
    {
        if (_tileMapTarget.TryGetComponent<IPositionProvider>(out var provider))
        {
            _tileMapTargetPosition = provider;
            return true;
        }
        else
        {
            Debug.LogError("IPositionProvider 컴포넌트를 찾을 수 없습니다!");
            return false;
        }
    }

    private bool CheckTileMapList()
    {
        if (TileMapList == null)
        {
            Debug.LogError("TileMapList에 TileMap들이 참조되지 않았습니다!");
            return false;
        }
        else
        {
            return true;
        }
    }

    public void Move()
    {
        if (CheckTileMap() == false) return;
        if (CheckTileMapList() == false) return;
        if (AllManager.instance.GameMode.GameMode != GameModeType.GAMEMODE_INGAME) return;

        // 현재 플레이어의 위치 가져오기
        Vector3 playerPosition = TileMapTargetPosition.Position;

        // 모든 타일맵에 대해 반복
        for (int i = 0; i < TileMapList.Count; i++)
        {
            GameObject tilemap = TileMapList[i];
            Vector3 mapPosition = tilemap.transform.position;

            // 타일맵의 경계 설정
            float leftBoundary = mapPosition.x - (TileMapSize * 0.5f); // 왼쪽 경계
            float rightBoundary = mapPosition.x + (TileMapSize * 0.5f); // 오른쪽 경계
            float bottomBoundary = mapPosition.y - (TileMapSize * 0.5f); // 아래쪽 경계
            float topBoundary = mapPosition.y + (TileMapSize * 0.5f); // 위쪽 경계

            // 플레이어가 경계를 벗어났을 때 타일맵을 이동시키기 위한 오프셋 초기화
            Vector3 moveOffset = Vector3.zero;

            // 플레이어가 왼쪽 경계를 벗어났을 경우
            if (playerPosition.x < leftBoundary)
            {
                // 타일맵을 왼쪽으로 이동
                moveOffset.x = -TileMapSize + TileMapOverlapOffset;
            }
            // 플레이어가 오른쪽 경계를 벗어났을 경우
            else if (playerPosition.x > rightBoundary)
            {
                // 타일맵을 오른쪽으로 이동
                moveOffset.x = TileMapSize - TileMapOverlapOffset;
            }

            // 플레이어가 아래쪽 경계를 벗어났을 경우
            if (playerPosition.y < bottomBoundary)
            {
                // 타일맵을 아래쪽으로 이동
                moveOffset.y = -TileMapSize + TileMapOverlapOffset;
            }
            // 플레이어가 위쪽 경계를 벗어났을 경우
            else if (playerPosition.y > topBoundary)
            {
                // 타일맵을 위쪽으로 이동
                moveOffset.y = TileMapSize - TileMapOverlapOffset;
            }

            // 플레이어가 경계를 벗어나 타일맵 이동이 필요한 경우
            if (moveOffset != Vector3.zero)
            {
                // 타일맵의 위치를 오프셋만큼 이동
                tilemap.transform.position += moveOffset;
            }
        }
    }
}
