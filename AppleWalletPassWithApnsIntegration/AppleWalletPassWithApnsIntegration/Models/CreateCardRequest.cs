using System.ComponentModel.DataAnnotations;

namespace AppleWalletPassWithApnsIntegration.Models;

/// <summary>
/// Запрос на создание карты
/// </summary>
public class CreateCardRequest
{
    /// <summary>
    /// Идентификатор карты
    /// </summary>
    public long CardId { get; set; }
}