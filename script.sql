USE [master]
GO
/****** Object:  Database [OrdenesTrabajoDB]    Script Date: 13/06/2025 10:22:19 ******/
CREATE DATABASE [OrdenesTrabajoDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'OrdenesTrabajoDB', FILENAME = N'L:\SQLDATA\OrdenesTrabajoDB.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'OrdenesTrabajoDB_log', FILENAME = N'L:\SQLLOG\OrdenesTrabajoDB_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [OrdenesTrabajoDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [OrdenesTrabajoDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [OrdenesTrabajoDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [OrdenesTrabajoDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [OrdenesTrabajoDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [OrdenesTrabajoDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [OrdenesTrabajoDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [OrdenesTrabajoDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [OrdenesTrabajoDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [OrdenesTrabajoDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [OrdenesTrabajoDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [OrdenesTrabajoDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [OrdenesTrabajoDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [OrdenesTrabajoDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [OrdenesTrabajoDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [OrdenesTrabajoDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [OrdenesTrabajoDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [OrdenesTrabajoDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [OrdenesTrabajoDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [OrdenesTrabajoDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [OrdenesTrabajoDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [OrdenesTrabajoDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [OrdenesTrabajoDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [OrdenesTrabajoDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [OrdenesTrabajoDB] SET RECOVERY FULL 
GO
ALTER DATABASE [OrdenesTrabajoDB] SET  MULTI_USER 
GO
ALTER DATABASE [OrdenesTrabajoDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [OrdenesTrabajoDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [OrdenesTrabajoDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [OrdenesTrabajoDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [OrdenesTrabajoDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [OrdenesTrabajoDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [OrdenesTrabajoDB] SET QUERY_STORE = OFF
GO
USE [OrdenesTrabajoDB]
GO
/****** Object:  Table [dbo].[Sistema]    Script Date: 13/06/2025 10:22:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sistema](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[estado] [int] NOT NULL,
 CONSTRAINT [PK__Sistema__3213E83F91CB1AA1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Supervisor]    Script Date: 13/06/2025 10:22:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Supervisor](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[idSupervisor] [char](8) NOT NULL,
	[nombre] [varchar](100) NOT NULL,
	[estado] [int] NOT NULL,
 CONSTRAINT [PK_Supervisor] PRIMARY KEY CLUSTERED 
(
	[idSupervisor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tecnico]    Script Date: 13/06/2025 10:22:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tecnico](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[idTecnico] [char](8) NOT NULL,
	[nombre] [varchar](100) NOT NULL,
	[estado] [int] NOT NULL,
 CONSTRAINT [PK_Tecnico] PRIMARY KEY CLUSTERED 
(
	[idTecnico] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 13/06/2025 10:22:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario](
	[id] [char](8) NOT NULL,
	[nombre] [varchar](100) NOT NULL,
	[tipo_persona_id] [int] NOT NULL,
	[sociedad_id] [int] NOT NULL,
	[area_id] [int] NOT NULL,
	[clave] [varbinary](32) NULL,
	[estado] [int] NOT NULL,
	[creador_id] [int] NULL,
 CONSTRAINT [PK__Usuario__3213E83F86A98DCD] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MotivoRechazo]    Script Date: 13/06/2025 10:22:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MotivoRechazo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[om] [varchar](20) NULL,
	[pos] [varchar](5) NULL,
	[motivo_rechazo] [varchar](200) NULL,
	[fecha_rechazo] [date] NULL,
 CONSTRAINT [PK_MotivoRechazo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Programacion]    Script Date: 13/06/2025 10:22:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Programacion](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[om] [varchar](20) NOT NULL,
	[pos] [varchar](5) NOT NULL,
	[actividad] [varchar](100) NOT NULL,
	[fecha_inicio_mas_temprano] [date] NULL,
	[fecha_fin_mas_temprano] [date] NULL,
	[status_sap] [varchar](50) NOT NULL,
	[campo_clasificacion] [varchar](50) NULL,
	[ubicacion_tecnica] [varchar](50) NULL,
	[denominacion_ubicacion_tecnica] [varchar](100) NULL,
	[equipo] [varchar](50) NULL,
	[denominacion_objeto_tecnico] [varchar](100) NULL,
	[clase_orden] [varchar](15) NULL,
	[aviso] [varchar](20) NULL,
	[puesto_trabajo] [varchar](15) NULL,
	[cantidad_persona_programada] [decimal](18, 2) NULL,
	[duracion_programada] [decimal](18, 2) NULL,
	[trabajo_real_programado] [decimal](18, 2) NULL,
	[supervisor_id] [char](8) NULL,
	[tecnico_id] [char](8) NULL,
	[idusuario] [char](8) NULL,
	[fecha_inicio_real] [date] NULL,
	[fecha_fin_real] [date] NULL,
	[cantidad_persona] [decimal](18, 2) NULL,
	[duracion_ejecutada] [decimal](18, 2) NULL,
	[trabajo_real] [decimal](18, 2) NULL,
	[idsistema] [int] NULL,
	[idubicacion] [int] NULL,
	[observacion] [varchar](200) NULL,
	[status_reporte] [char](1) NULL,
	[status_tecnico] [char](1) NULL,
	[status_aprobacion_supervisor] [char](1) NULL,
	[estatus_backlog] [char](1) NULL,
	[imagen_nombre] [varchar](200) NULL,
 CONSTRAINT [PK__Programa__3213E83F7199EDF3] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ubicacion]    Script Date: 13/06/2025 10:22:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ubicacion](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](50) NOT NULL,
	[estado] [int] NOT NULL,
 CONSTRAINT [PK__Ubicacio__3213E83FAA91D179] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sociedad]    Script Date: 13/06/2025 10:22:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sociedad](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](25) NOT NULL,
	[estado] [int] NOT NULL,
 CONSTRAINT [PK__Sociedad__3213E83F10F6B762] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VisualizarProgramas]    Script Date: 13/06/2025 10:22:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[VisualizarProgramas] 
AS
SELECT p.id,p.om,p.pos,p.actividad,p.fecha_inicio_mas_temprano,p.fecha_fin_mas_temprano,p.status_sap,p.campo_clasificacion,p.ubicacion_tecnica,p.denominacion_ubicacion_tecnica,p.equipo,p.denominacion_objeto_tecnico,
        p.clase_orden,p.aviso,p.puesto_trabajo,p.cantidad_persona_programada,p.duracion_programada,p.trabajo_real_programado,p.supervisor_id, s.nombre supervisor,p.tecnico_id, t.nombre tecnico,p.idusuario,u.nombre usuario,
        p.fecha_inicio_real,p.fecha_fin_real,p.cantidad_persona,p.duracion_ejecutada,p.trabajo_real,q.Nombre sistema,r.nombre ubicacion,sociedad_id, p.observacion,

        CASE WHEN p.status_reporte = '0' AND p.status_tecnico = '0' AND p.status_aprobacion_supervisor = '0' AND p.estatus_backlog = '0' THEN 'planificado' -- Cuando es subido por el planner como carga inicial del programa
             WHEN p.status_reporte = '0' AND p.status_tecnico = '1' AND p.status_aprobacion_supervisor = '0' AND p.estatus_backlog = '0' THEN 'ejecutado' -- Cuando el tecnico ejecuta pero aun no esta tratado por el supervisor
             WHEN p.status_reporte = '0' AND p.status_tecnico = '2' AND p.status_aprobacion_supervisor = '0' AND p.estatus_backlog = '0' THEN 'no_ejecutado' -- Cuando el tecnico no ejecuta pero aun no esta tratado por el supervisor
             WHEN p.status_reporte = '1' AND p.status_tecnico = '1' AND p.status_aprobacion_supervisor = '1' AND p.estatus_backlog = '0' THEN 'culminado' -- Cuando el tecnico ejecuta y el supervisor lo aprueba
             WHEN p.status_reporte = '0' AND p.status_tecnico = '0' AND p.status_aprobacion_supervisor = '0' AND p.estatus_backlog = '3' THEN 'backlog' -- -- Cuando el tecnico no ejecuta y el supervisor lo aprueba
        ELSE 'estado_incorrecto' END AS proceso,

        CASE WHEN p.status_reporte = '0' AND p.status_tecnico = '0' AND p.status_aprobacion_supervisor = '0' AND p.estatus_backlog = '0' THEN 'planificado' -- Cuando es subido por el planner como carga inicial del programa
             WHEN p.status_reporte = '0' AND p.status_tecnico = '1' AND p.status_aprobacion_supervisor = '0' AND p.estatus_backlog = '0' THEN 'ejecutado,no_ejecutado' -- Cuando el tecnico ejecuta pero aun no esta tratado por el supervisor
             WHEN p.status_reporte = '0' AND p.status_tecnico = '2' AND p.status_aprobacion_supervisor = '0' AND p.estatus_backlog = '0' THEN 'ejecutado,no_ejecutado' -- Cuando el tecnico no ejecuta pero aun no esta tratado por el supervisor
             WHEN p.status_reporte = '1' AND p.status_tecnico = '1' AND p.status_aprobacion_supervisor = '1' AND p.estatus_backlog = '0' THEN 'culminado' -- Cuando el tecnico ejecuta y el supervisor lo aprueba
             WHEN p.status_reporte = '0' AND p.status_tecnico = '0' AND p.status_aprobacion_supervisor = '0' AND p.estatus_backlog = '3' THEN 'backlog' -- -- Cuando el tecnico no ejecuta y el supervisor lo aprueba
        ELSE 'estado_incorrecto' END AS estado,
            mr.motivo_rechazo AS motivo_rechazo,
            p.imagen_nombre as imagen_nombre

FROM [OrdenesTrabajoDB].[dbo].[Programacion] as p
LEFT JOIN [dbo].[Supervisor] as s ON s.idSupervisor = p.supervisor_id
LEFT JOIN [dbo].[Tecnico] as t ON t.idTecnico = p.tecnico_id
LEFT JOIN [dbo].[Usuario] as u ON u.id = p.idusuario
LEFT JOIN [dbo].[Sistema] as q ON q.id = p.idsistema
LEFT JOIN [dbo].[Ubicacion] as r ON r.id = p.idubicacion
LEFT JOIN [dbo].[Sociedad] as v ON v.id = u.sociedad_id
LEFT JOIN
    (
        SELECT om, pos, motivo_rechazo
        FROM (
            SELECT om, pos, motivo_rechazo, 
                   ROW_NUMBER() OVER (PARTITION BY om, pos ORDER BY Id DESC) AS rn
            FROM [dbo].[MotivoRechazo]
        ) AS subquery
        WHERE rn = 1
    ) AS mr ON mr.om = p.om AND mr.pos = p.pos
GO
/****** Object:  Table [dbo].[Area]    Script Date: 13/06/2025 10:22:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Area](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](100) NOT NULL,
	[gerencia_id] [int] NOT NULL,
	[estado] [int] NOT NULL,
 CONSTRAINT [PK__Area__3213E83F75D00AB1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Gerencia]    Script Date: 13/06/2025 10:22:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Gerencia](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](50) NOT NULL,
	[estado] [int] NOT NULL,
 CONSTRAINT [PK__Gerencia__3213E83F7579668A] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrdenesTrabajo]    Script Date: 13/06/2025 10:22:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrdenesTrabajo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrdenMantenimiento] [nvarchar](50) NOT NULL,
	[Descripcion] [nvarchar](255) NOT NULL,
	[Estado] [nvarchar](50) NOT NULL,
	[FechaInicio] [date] NOT NULL,
	[FechaFin] [date] NULL,
	[HoraInicio] [time](7) NULL,
	[HoraFin] [time](7) NULL,
	[Observaciones] [nvarchar](500) NULL,
	[Supervisor] [nvarchar](100) NOT NULL,
	[Tecnico] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoPersonas]    Script Date: 13/06/2025 10:22:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoPersonas](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[tipo] [varchar](40) NOT NULL,
	[estado] [int] NOT NULL,
 CONSTRAINT [PK__TipoPers__3213E83FF1948053] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[MotivoRechazo] ADD  CONSTRAINT [DEFAULT_MotivoRechazo_fecha_rechazo]  DEFAULT (getdate()) FOR [fecha_rechazo]
GO
ALTER TABLE [dbo].[Programacion] ADD  CONSTRAINT [DF_Programacion_Cantidad_persona]  DEFAULT ((0)) FOR [cantidad_persona]
GO
ALTER TABLE [dbo].[Programacion] ADD  CONSTRAINT [DF_Programacion_Duracion_Ejecutada]  DEFAULT ((0)) FOR [duracion_ejecutada]
GO
ALTER TABLE [dbo].[Programacion] ADD  CONSTRAINT [DF_Programacion_Trabajo_real]  DEFAULT ((0)) FOR [trabajo_real]
GO
ALTER TABLE [dbo].[Programacion] ADD  CONSTRAINT [DF_Status_reporte]  DEFAULT ('0') FOR [status_reporte]
GO
ALTER TABLE [dbo].[Programacion] ADD  CONSTRAINT [DF_Status_tecnico]  DEFAULT ('0') FOR [status_tecnico]
GO
ALTER TABLE [dbo].[Programacion] ADD  CONSTRAINT [DF_Status_aprobacion_supervisor]  DEFAULT ('0') FOR [status_aprobacion_supervisor]
GO
ALTER TABLE [dbo].[Programacion] ADD  CONSTRAINT [DF_Estatus_backlog]  DEFAULT ('0') FOR [estatus_backlog]
GO
ALTER TABLE [dbo].[Area]  WITH CHECK ADD  CONSTRAINT [FK__Area__gerencia_i__2F10007B] FOREIGN KEY([gerencia_id])
REFERENCES [dbo].[Gerencia] ([id])
GO
ALTER TABLE [dbo].[Area] CHECK CONSTRAINT [FK__Area__gerencia_i__2F10007B]
GO
ALTER TABLE [dbo].[Programacion]  WITH NOCHECK ADD  CONSTRAINT [FK_Programacion_Sistema] FOREIGN KEY([idsistema])
REFERENCES [dbo].[Sistema] ([id])
GO
ALTER TABLE [dbo].[Programacion] CHECK CONSTRAINT [FK_Programacion_Sistema]
GO
ALTER TABLE [dbo].[Programacion]  WITH NOCHECK ADD  CONSTRAINT [FK_Programacion_Supervisor] FOREIGN KEY([supervisor_id])
REFERENCES [dbo].[Supervisor] ([idSupervisor])
GO
ALTER TABLE [dbo].[Programacion] CHECK CONSTRAINT [FK_Programacion_Supervisor]
GO
ALTER TABLE [dbo].[Programacion]  WITH NOCHECK ADD  CONSTRAINT [FK_Programacion_Tecnico] FOREIGN KEY([tecnico_id])
REFERENCES [dbo].[Tecnico] ([idTecnico])
GO
ALTER TABLE [dbo].[Programacion] CHECK CONSTRAINT [FK_Programacion_Tecnico]
GO
ALTER TABLE [dbo].[Programacion]  WITH NOCHECK ADD  CONSTRAINT [FK_Programacion_Ubicacion] FOREIGN KEY([idubicacion])
REFERENCES [dbo].[Ubicacion] ([id])
GO
ALTER TABLE [dbo].[Programacion] CHECK CONSTRAINT [FK_Programacion_Ubicacion]
GO
ALTER TABLE [dbo].[Programacion]  WITH NOCHECK ADD  CONSTRAINT [FK_Programacion_Usuario] FOREIGN KEY([idusuario])
REFERENCES [dbo].[Usuario] ([id])
GO
ALTER TABLE [dbo].[Programacion] CHECK CONSTRAINT [FK_Programacion_Usuario]
GO
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK__Usuario__area_id__33D4B598] FOREIGN KEY([area_id])
REFERENCES [dbo].[Area] ([id])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK__Usuario__area_id__33D4B598]
GO
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK__Usuario__socieda__32E0915F] FOREIGN KEY([sociedad_id])
REFERENCES [dbo].[Sociedad] ([id])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK__Usuario__socieda__32E0915F]
GO
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK__Usuario__tipo_pe__31EC6D26] FOREIGN KEY([tipo_persona_id])
REFERENCES [dbo].[TipoPersonas] ([id])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK__Usuario__tipo_pe__31EC6D26]
GO
USE [master]
GO
ALTER DATABASE [OrdenesTrabajoDB] SET  READ_WRITE 
GO
