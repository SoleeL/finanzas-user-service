using finanzas_user_service.Data.Entity;

namespace finanzas_user_service.Repository;

public interface IUserRepository
{
    Task<string> RegisterUserAsync(User user);
    
    //     - GET /api/user/me
//     - Descripción: Obtiene la información del perfil del usuario autenticado. Utilizado para obtener información del perfil del usuario actual sin necesidad de especificar el userId.
//
    Task<User> GetAuthenticatedUserByTokenAsync(string token);
    
// - GET /api/user/{userId}
// - Descripción: Obtiene la información del usuario especificado.
// - Parámetros: userId (ID del usuario)
    Task<User?> GetUserByIdAsync(Guid guid);
    
    //     - GET /api/users
//     - Descripción: Lista todos los usuarios. Puede incluir parámetros para paginación y filtrado.
// - Parámetros: Opcionalmente, page, size, sort, filter
    Task<List<User>> GetAllUsersAsync();
    
    //         - PUT /api/user/{userId}
// - Descripción: Actualiza la información del perfil del usuario.
// - Cuerpo de Solicitud: Datos actualizados del usuario.
// - Parámetros: userId (ID del usuario a actualizar)
//
    Task<User> UpdateUserByIdAsync(string id, User user);
    
//     - DELETE /api/user/{userId}
// - Descripción: Elimina un usuario del sistema.
// - Parámetros: userId (ID del usuario a eliminar)
    Task<User> DeletUserByIdAsync(string id);
    
//     - POST /api/user/forgot-password
//     - Descripción: Inicia el proceso de recuperación de contraseña. Puede enviar un enlace de recuperación al correo electrónico del usuario.
// - Cuerpo de Solicitud: Correo electrónico del usuario
    Task<string> ForgotPasswordUserAsync(string id);

//     - POST /api/user/reset-password
//     - Descripción: Restablece la contraseña del usuario utilizando un token de recuperación.
// - Cuerpo de Solicitud: Token de recuperación, nueva contraseña
    Task<User> ResetPasswordUserByTokenAsync(string token, string newPassword);
    
//     - POST /api/user/confirm-email
//     - Descripción: Confirma la dirección de correo electrónico del usuario después del registro.
// - Cuerpo de Solicitud: Token de confirmación
    Task<User> ConfirmUserByEmailAsync(string token, string email);
}