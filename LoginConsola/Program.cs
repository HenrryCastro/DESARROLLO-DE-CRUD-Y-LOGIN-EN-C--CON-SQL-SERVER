using System;
using System.Linq;
using LoginConsola.Models;

class Program
{
    static void Main(string[] args)
    {
        int opcion = 0;
        do
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("    SISTEMA DE GESTION - LOGIN CONSOLA  ");
            Console.WriteLine("========================================");
            Console.WriteLine("1. Registrar usuario");
            Console.WriteLine("2. Listar todos los usuarios");
            Console.WriteLine("3. Buscar usuario por ID");
            Console.WriteLine("4. Actualizar datos de un usuario");
            Console.WriteLine("5. Eliminar usuario");
            Console.WriteLine("6. Iniciar Sesion (Login)");
            Console.WriteLine("7. Salir");
            Console.WriteLine("========================================");
            Console.Write("Seleccione una opcion: ");

            if (int.TryParse(Console.ReadLine(), out opcion))
            {
                switch (opcion)
                {
                    case 1:
                        RegistrarUsuario();
                        break;
                    case 2:
                        ListarUsuarios();
                        break;
                    case 3:
                        BuscarUsuarioPorId();
                        break;
                    case 4:
                        ActualizarUsuario();
                        break;
                    case 5:
                        EliminarUsuario();
                        break;
                    case 6:
                        IniciarSesion();
                        break;
                    case 7:
                        Console.WriteLine("\nSaliendo del sistema...");
                        break;
                    default:
                        Console.WriteLine("\nOpcion no valida. Intente de nuevo.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("\nPor favor, ingrese un numero valido.");
            }

            if (opcion != 7)
            {
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
            }

        } while (opcion != 7);
    }

    static void RegistrarUsuario()
    {
        Console.Clear();
        Console.WriteLine("--- REGISTRAR NUEVO USUARIO ---");
        Console.Write("Ingrese nombre de usuario: ");
        string username = Console.ReadLine()?.Trim() ?? "";

        Console.Write("Ingrese contrasena: ");
        string password = Console.ReadLine()?.Trim() ?? "";

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Console.WriteLine("Error: Los campos no pueden estar vacios.");
            return;
        }

        using (var db = new DbLoginContext())
        {
            if (db.Usuarios.Any(u => u.NombreUsuario == username))
            {
                Console.WriteLine("El nombre de usuario ya existe en la base de datos.");
                return;
            }

            db.Usuarios.Add(new Usuario { NombreUsuario = username, Password = password });
            db.SaveChanges();
            Console.WriteLine("Usuario registrado exitosamente en SQL Server.");
        }
    }

    static void ListarUsuarios()
    {
        Console.Clear();
        Console.WriteLine("--- LISTA DE USUARIOS REGISTRADOS ---");

        using (var db = new DbLoginContext())
        {
            var usuarios = db.Usuarios.ToList();

            if (!usuarios.Any())
            {
                Console.WriteLine("No hay usuarios registrados.");
                return;
            }

            Console.WriteLine("{0,-5} | {1,-25}", "ID", "NOMBRE DE USUARIO");
            Console.WriteLine(new string('-', 35));

            foreach (var u in usuarios)
            {
                Console.WriteLine("{0,-5} | {1,-25}", u.Id, u.NombreUsuario);
            }
        }
    }

    static void BuscarUsuarioPorId()
    {
        Console.Clear();
        Console.WriteLine("--- BUSCAR USUARIO POR ID ---");
        Console.Write("Ingrese el ID del usuario: ");

        if (int.TryParse(Console.ReadLine(), out int id))
        {
            using (var db = new DbLoginContext())
            {
                var usuario = db.Usuarios.Find(id);
                if (usuario != null)
                {
                    Console.WriteLine("\nUsuario Encontrado:");
                    Console.WriteLine($"ID: {usuario.Id}");
                    Console.WriteLine($"Nombre de Usuario: {usuario.NombreUsuario}");
                }
                else
                {
                    Console.WriteLine("No se encontro ningun usuario con ese ID.");
                }
            }
        }
        else
        {
            Console.WriteLine("ID invalido.");
        }
    }

    static void ActualizarUsuario()
    {
        Console.Clear();
        Console.WriteLine("--- ACTUALIZAR USUARIO ---");
        Console.Write("Ingrese el ID del usuario a actualizar: ");

        if (int.TryParse(Console.ReadLine(), out int id))
        {
            using (var db = new DbLoginContext())
            {
                var usuario = db.Usuarios.Find(id);
                if (usuario != null)
                {
                    Console.WriteLine($"Usuario actual: {usuario.NombreUsuario}");
                    Console.Write("Ingrese el nuevo nombre de usuario: ");
                    string nuevoNombre = Console.ReadLine()?.Trim() ?? "";

                    Console.Write("Ingrese la nueva contrasena: ");
                    string nuevaPassword = Console.ReadLine()?.Trim() ?? "";

                    if (!string.IsNullOrEmpty(nuevoNombre)) usuario.NombreUsuario = nuevoNombre;
                    if (!string.IsNullOrEmpty(nuevaPassword)) usuario.Password = nuevaPassword;

                    db.SaveChanges();
                    Console.WriteLine("Datos de usuario actualizados correctamente.");
                }
                else
                {
                    Console.WriteLine("No se encontro un usuario con ese ID.");
                }
            }
        }
        else
        {
            Console.WriteLine("ID invalido.");
        }
    }

    static void EliminarUsuario()
    {
        Console.Clear();
        Console.WriteLine("--- ELIMINAR USUARIO ---");
        Console.Write("Ingrese el ID del usuario a eliminar: ");

        if (int.TryParse(Console.ReadLine(), out int id))
        {
            using (var db = new DbLoginContext())
            {
                var usuario = db.Usuarios.Find(id);
                if (usuario != null)
                {
                    Console.WriteLine($"¿Esta seguro de eliminar al usuario '{usuario.NombreUsuario}'? (S/N): ");
                    string confirmacion = Console.ReadLine()?.Trim().ToUpper() ?? "N";

                    if (confirmacion == "S")
                    {
                        db.Usuarios.Remove(usuario);
                        db.SaveChanges();
                        Console.WriteLine("Usuario eliminado correctamente.");
                    }
                    else
                    {
                        Console.WriteLine("Operacion cancelada.");
                    }
                }
                else
                {
                    Console.WriteLine("No se encontro un usuario con ese ID.");
                }
            }
        }
        else
        {
            Console.WriteLine("ID invalido.");
        }
    }

    static void IniciarSesion()
    {
        Console.Clear();
        Console.WriteLine("--- INICIO DE SESION ---");
        Console.Write("Usuario: ");
        string username = Console.ReadLine()?.Trim() ?? "";

        Console.Write("Contrasena: ");
        string password = Console.ReadLine()?.Trim() ?? "";

        using (var db = new DbLoginContext())
        {
            var usuarioValido = db.Usuarios
                .FirstOrDefault(u => u.NombreUsuario == username && u.Password == password);

            if (usuarioValido != null)
            {
                Console.WriteLine($"\n¡Bienvenido, {usuarioValido.NombreUsuario}! Acceso concedido.");
            }
            else
            {
                Console.WriteLine("\nAcceso denegado: Usuario o contraseña incorrectos.");
            }
        }
    }
}
