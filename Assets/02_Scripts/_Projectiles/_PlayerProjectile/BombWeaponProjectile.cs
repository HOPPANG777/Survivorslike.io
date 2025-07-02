using UnityEngine;

// 포물선을 그리며 떨어지는 폭탄 투사체

public class BombWeaponProjectile : ProjectileBase
{
    // 회전 속도
    [SerializeField] private float _rotationSpeed = 360f;
    // 낙하 속도 배율
    [SerializeField] private float _dropSpeedMultiplier = 2f;

    // 베지어 곡선 네 지점
    private Vector3 _p1;
    private Vector3 _p2;
    private Vector3 _p3;
    private Vector3 _p4;
    // 경과 시간
    private float _timer;
    // 전체 비행 시간
    private float _lifeTime;

    // 매 프레임 갱신 처리
    protected override void Update()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;
     
        base.Update();
    }

    // 곡선 경로 초기화
    public void Initialize(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {
        _p1 = p1;
        _p2 = p2;
        _p3 = p3;
        _p4 = p4;
        _timer = 0f;
    }

    // 기본 상태 설정
    public override void SetStatus(WeaponBase ownerWeapon)
    {
        base.SetStatus(ownerWeapon);
        _lifeTime = Attributes.LifeTime;
        _timer = 0f;
        RemainingLifeTime = _lifeTime / _dropSpeedMultiplier;
    }

    // 베지어 곡선을 따라 이동
    protected override void Move()
    {
        _timer += Time.deltaTime * _dropSpeedMultiplier;
        float t = _lifeTime > 0f ? Mathf.Clamp01(_timer / _lifeTime) : 1f;
        Vector3 pos = CalculateBezier(_p1, _p2, _p3, _p4, t);
        this.transform.position = pos;
        this.transform.Rotate(Vector3.forward, _rotationSpeed * Time.deltaTime);
    }

    // 베지어 곡선 계산 함수
    private static Vector3 CalculateBezier(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float value)
    {
        Vector3 a = Vector3.Lerp(p1, p2, value);
        Vector3 b = Vector3.Lerp(p2, p3, value);
        Vector3 c = Vector3.Lerp(p3, p4, value);

        Vector3 d = Vector3.Lerp(a, b, value);
        Vector3 e = Vector3.Lerp(b, c, value);

        return Vector3.Lerp(d, e, value);
    }

    // 생존 시간 초과 시 사망 처리
    public override void CheckDead()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;
        base.CheckDead();
        if (_timer >= _lifeTime)
        {
            Dead();
        }
    }

    public override void Dead()
    {
        base.Dead();
    }

    // 사운드 재생
    public override void SoundPlay()
    {
        base.SoundPlay();
    }

    // 사운드 중지
    public override void SoundStop()
    {
        base.SoundStop();
    }
}
