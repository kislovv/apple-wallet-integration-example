using System.ComponentModel.DataAnnotations;


namespace AppleWalletPassWithApnsIntegration.Models;

/// <summary>
/// Запрос на создание карты
/// </summary>
public class CreateCardRequest
{
    /// <summary>
    /// Хеш карты
    /// </summary>
    public string UserHashId { get; set; }
    
    /// <summary>
    /// Идентификатор партнера привязанный к карте
    /// </summary>
    public long PartnerId { get; set; }
    
    
}