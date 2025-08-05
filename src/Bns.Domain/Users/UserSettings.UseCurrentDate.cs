namespace Bns.Domain.Users;

public record UseCurrentDate
    (
    bool ImplicitPanels = true, // Использовать текущую дату для панелей
    bool UserPanels = true, // Использовать текущую дату для пользовательских панелей
    int CurrentDateOffset = 0
    );
