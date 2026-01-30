# SAP B1 Training

> Proyecto de entrenamiento de SAP Business One con C# y SQL Server

## Descripción

Este proyecto es una **aplicación de consola en C#** que simula operaciones básicas de SAP Business One para **Business Partners**, incluyendo auditoría de cambios.  
Permite a los usuarios crear, actualizar, eliminar y listar Business Partners, registrando automáticamente quién realizó cada acción y cuándo.

### Funcionalidades principales

- Crear, actualizar y eliminar Business Partners (clientes/proveedores)  
- Auditoría de todas las acciones (CREATE, UPDATE, DELETE)  
- Registro del usuario que ejecuta cada acción  
- Visualización de auditoría por `CardCode`  
- Validaciones tipo SAP B1 en nombres y TaxId  

### Tecnologías y Herramientas

- **Lenguaje:** C# 11  
- **Framework:** .NET 8  
- **Base de datos:** SQL Server  
- **Control de versiones:** Git / GitHub  
- **IDE recomendado:** Visual Studio o VS Code  

### Estructura del proyecto

# SAP B1 Training

> Proyecto de entrenamiento de SAP Business One con C# y SQL Server

## Descripción

Este proyecto es una **aplicación de consola en C#** que simula operaciones básicas de SAP Business One para **Business Partners**, incluyendo auditoría de cambios.  
Permite a los usuarios crear, actualizar, eliminar y listar Business Partners, registrando automáticamente quién realizó cada acción y cuándo.

### Funcionalidades principales

- Crear, actualizar y eliminar Business Partners (clientes/proveedores)  
- Auditoría de todas las acciones (CREATE, UPDATE, DELETE)  
- Registro del usuario que ejecuta cada acción  
- Visualización de auditoría por `CardCode`  
- Validaciones tipo SAP B1 en nombres y TaxId  

### Tecnologías y Herramientas

- **Lenguaje:** C# 11  
- **Framework:** .NET 8  
- **Base de datos:** SQL Server  
- **Control de versiones:** Git / GitHub  
- **IDE recomendado:** Visual Studio o VS Code  

### Estructura del proyecto

SAP-B1-TRAINING/
│
├─ src/ConsoleApp/
│ ├─ Data/ # Conexión y inicialización de la base de datos
│ ├─ Models/ # Modelos AuditLog y BusinessPartner
│ ├─ Services/ # Lógica de negocio y auditoría
│ ├─ Security/ # Gestión del usuario activo (SapUserContext)
│ └─ Program.cs # Punto de entrada de la aplicación
│
├─ .gitignore
└─ sap-b1-training.sln


### Cómo ejecutar el proyecto

1. Clonar el repositorio:
git clone https://github.com/V1c02/sap-b1-training.git
cd sap-b1-training/src/ConsoleApp

2. Restaurar paquetes y ejecutar la aplicación:
dotnet restore
dotnet run

3. Ingresar un usuario para registrar la auditoría y seguir las opciones del menú:
  • 1: Crear Business Partner
  • 2: Listar Business Partners
  • 3: Salir
  • 4: Actualizar Business Partner
  • 5: Eliminar Business Partner
  • 6: Ver auditoría

Buenas prácticas incluidas

  • Auditoría centralizada con usuario y timestamp
  • Validaciones de campos obligatorios y longitud máxima
  • Manejo de errores de base de datos
  • Uso de Git y .gitignore configurado para Visual Studio/VSCode

Autor: Victor L
Email: hugolc98@hotmail.com
Repositorio: https://github.com/V1c02/sap-b1-training
