namespace AppleWalletPassWithApnsIntegration.Models;

/// <summary>
/// Запрос на создание Pass AppleWallet
/// </summary>
public class PassRequest
{
    /// <summary>
    /// Хеш карты пользователя
    /// </summary>
    public string UserHashId { get; set; }
    /// <summary>
    /// Название девайса в котором необходимо создать pass
    /// </summary>
    public string DeviceName { get; set; }
}