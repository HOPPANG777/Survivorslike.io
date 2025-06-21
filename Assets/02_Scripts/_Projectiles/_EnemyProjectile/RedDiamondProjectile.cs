using UnityEngine;
using System.Collections.Generic;

//==========================================================================
// RedDiamondProjectile
//==========================================================================
// 일정 시간 후 지정한 범위 안의 플레이어에게 피해를 주는 적 투사체
// - 스폰 딜레이 동안 범위 표시 스프라이트가 활성화됩니다.
// - 딜레이가 끝나면 투사체 스프라이트가 커지면서 공격 범위를 나타냅니다.
// - 최대 크기에 도달하면 범위 내의 플레이어를 탐색하여 데미지를 입힙니다.
//==========================================================================
public class RedDiamondProjectile : ProjectileBase
{
    // 스폰 후 공격까지 대기할 시간
    [SerializeField] private float _spawnDelay = 3f;
    // 범위를 표현할 스프라이트 렌더러
    [SerializeField] private SpriteRenderer _rangeRenderer;
    // 투사체 자체 스프라이트 렌더러
    [SerializeField] private SpriteRenderer _projectileRenderer;

    // 현재 누적된 시간
    private float _timer;
    // 범위 표시 후 커지는 단계로 들어갔는지 여부
    private bool _expanding;
    // 투사체가 도달해야 할 최종 스케일 값
    private float _targetScale;

    // 기본 상태 설정
    public override void SetStatus()
    {
        base.SetStatus();

        if (_projectileRenderer == null)
            _projectileRenderer = GetComponent<SpriteRenderer>();

        if (_rangeRenderer != null) return;

        // 프리팹에서 할당되지 않았다면 자식에서 찾아준다
        var renderers = GetComponentsInChildren<SpriteRenderer>(true);
        for (int i = 0; i < renderers.Length; ++i)
        {
            var r = renderers[i];
            if (r != _projectileRenderer)
            {
                _rangeRenderer = r;
                break;
            }
        }

        // 범위 스프라이트는 초기에는 꺼두었다가 딜레이 중 표시
        if (_rangeRenderer != null)
            _rangeRenderer.gameObject.SetActive(false);

        if (_projectileRenderer.drawMode != SpriteDrawMode.Sliced)
            _projectileRenderer.drawMode = SpriteDrawMode.Sliced;

        _projectileRenderer.size = Vector2.zero;
        _timer = 0f;
        _expanding = false;
        // 속성의 폭발 반경을 기준으로 최종 스케일 계산
        _targetScale = Attributes.ExplosionRadius;
    }

    // 매 프레임 상태 갱신
    protected override void Update()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;

        _timer += Time.deltaTime;

        if (!_expanding)
        {
            // 스폰 딜레이 동안 범위 스프라이트 표시
            if (_rangeRenderer != null && !_rangeRenderer.gameObject.activeSelf)
            {
                _rangeRenderer.gameObject.SetActive(true);
                _rangeRenderer.transform.localScale = Vector3.one * _targetScale;
            }

            if (_timer >= _spawnDelay)
            {
                // 딜레이가 끝나면 확대 단계로 진입
                _timer = 0f;
                _expanding = true;
            }
        }
        else
        {
            // 투사체 스프라이트 확대
            Vector2 size = _projectileRenderer.size;
            float newSize = Mathf.MoveTowards(size.x, _targetScale, Time.deltaTime * _targetScale);
            size = Vector2.one * newSize;
            _projectileRenderer.size = size;

            if (newSize >= _targetScale)
            {
                // 최대 크기에 도달하면 공격 판정 후 소멸
                DamagePlayerInRange();
                Dead();
            }
        }

        UpdateCameraBound();
    }

    // 범위 안에 플레이어가 존재하면 데미지를 가함
    private void DamagePlayerInRange()
    {
        // 폭발 범위와 플레이어 사이의 거리를 비교
        float range = Attributes.ExplosionRadius;
        float sqrRange = range * range;
        Vector3 pos = this.transform.position;

        // 현재 게임모드에서 플레이어 객체를 가져옴
        GameObject playerObj = AllManager.instance.GamemodeManager.Player;
        if (playerObj == null) return;

        PlayerUnit player = playerObj.GetComponent<PlayerUnit>();
        if (player == null) return;

        // 범위 내에 있으면 데미지 부여
        Vector3 delta = player.transform.position - pos;
        if (delta.sqrMagnitude <= sqrRange) player.Hitted(AttackPower); 
    }
}

