namespace FastWiki.Service;

public class JwtOptions
{
    public const string Name = "Jwt";
    
    /// <summary>
    /// ��Կ
    /// </summary>
    public static string Secret { get; set; }

    /// <summary>
    /// ��Чʱ�䣨Сʱ��
    /// </summary>
    public static int EffectiveHours { get; set; }
}