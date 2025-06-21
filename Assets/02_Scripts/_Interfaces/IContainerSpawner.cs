/// <summary>
/// 컨테이너 스폰 기능을 제공하는 인터페이스
/// </summary>
public interface IContainerSpawner
{
    /// <summary>
    /// 스폰 로직 초기화
    /// </summary>
    void Init(ISpawnPositionProvider provider);
}
