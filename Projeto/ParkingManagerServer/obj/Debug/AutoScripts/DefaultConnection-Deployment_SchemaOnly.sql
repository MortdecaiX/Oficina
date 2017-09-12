SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ContextKey] [nvarchar](300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Name] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ClaimType] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ClaimValue] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ProviderKey] [nvarchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[UserId] [nvarchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[RoleId] [nvarchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Email] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SecurityStamp] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[PhoneNumber] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EstacionamentoModels](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Nome] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[LocalizacaoImagem_Latitude] [float] NOT NULL,
	[LocalizacaoImagem_Longitude] [float] NOT NULL,
	[LocalizacaoImagem_Altitude] [float] NOT NULL,
	[Localizacao_Latitude] [float] NOT NULL,
	[Localizacao_Longitude] [float] NOT NULL,
	[Localizacao_Altitude] [float] NOT NULL,
	[Responsavel_Id] [bigint] NULL,
	[ImagemBase64] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ImagemAltura] [bigint] NOT NULL,
	[ImagemLargura] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.EstacionamentoModels] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OcupacaoModels](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[DataEntrada] [datetime] NOT NULL,
	[DataSaida] [datetime] NOT NULL,
	[Usuario_Id] [bigint] NULL,
	[Veiculo_Id] [bigint] NULL,
 CONSTRAINT [PK_dbo.OcupacaoModels] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PontoModels](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Localizacao_Latitude] [float] NOT NULL,
	[Localizacao_Longitude] [float] NOT NULL,
	[Localizacao_Altitude] [float] NOT NULL,
	[Entrada] [bit] NOT NULL,
	[Saida] [bit] NOT NULL,
	[PontoModel_Id] [bigint] NULL,
	[EstacionamentoModel_Id] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.PontoModels] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReservaModels](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Data] [datetime] NOT NULL,
	[DataEntrada] [datetime] NOT NULL,
	[DataExpiracao] [datetime] NOT NULL,
	[DataSaida] [datetime] NOT NULL,
	[Usuario_Id] [bigint] NULL,
	[Veiculo_Id] [bigint] NULL,
 CONSTRAINT [PK_dbo.ReservaModels] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsuarioModels](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Nome] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Email] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Senha] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CPF] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_dbo.UsuarioModels] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsuarioVeiculo](
	[UsuarioRefId] [bigint] NOT NULL,
	[VeiculoRefId] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.UsuarioVeiculo] PRIMARY KEY CLUSTERED 
(
	[UsuarioRefId] ASC,
	[VeiculoRefId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VagaModels](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Numero] [bigint] NOT NULL,
	[Tipo] [int] NOT NULL,
	[Localizacao_Latitude] [float] NOT NULL,
	[Localizacao_Longitude] [float] NOT NULL,
	[Localizacao_Altitude] [float] NOT NULL,
	[Pavimento] [int] NOT NULL,
	[Ocupacao_Id] [bigint] NULL,
	[Reserva_Id] [bigint] NULL,
	[Responsavel_Id] [bigint] NULL,
	[PontoModel_Id] [bigint] NULL,
 CONSTRAINT [PK_dbo.VagaModels] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VeiculoModels](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Placa] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Marca] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Modelo] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_dbo.VeiculoModels] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_PADDING ON

GO
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
SET ANSI_PADDING ON

GO
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
SET ANSI_PADDING ON

GO
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
SET ANSI_PADDING ON

GO
CREATE NONCLUSTERED INDEX [IX_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
SET ANSI_PADDING ON

GO
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserRoles]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
SET ANSI_PADDING ON

GO
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
CREATE NONCLUSTERED INDEX [IX_Responsavel_Id] ON [dbo].[EstacionamentoModels]
(
	[Responsavel_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
CREATE NONCLUSTERED INDEX [IX_Usuario_Id] ON [dbo].[OcupacaoModels]
(
	[Usuario_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
CREATE NONCLUSTERED INDEX [IX_Veiculo_Id] ON [dbo].[OcupacaoModels]
(
	[Veiculo_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
CREATE NONCLUSTERED INDEX [IX_EstacionamentoModel_Id] ON [dbo].[PontoModels]
(
	[EstacionamentoModel_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
CREATE NONCLUSTERED INDEX [IX_PontoModel_Id] ON [dbo].[PontoModels]
(
	[PontoModel_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
CREATE NONCLUSTERED INDEX [IX_Usuario_Id] ON [dbo].[ReservaModels]
(
	[Usuario_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
CREATE NONCLUSTERED INDEX [IX_Veiculo_Id] ON [dbo].[ReservaModels]
(
	[Veiculo_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
CREATE NONCLUSTERED INDEX [IX_UsuarioRefId] ON [dbo].[UsuarioVeiculo]
(
	[UsuarioRefId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
CREATE NONCLUSTERED INDEX [IX_VeiculoRefId] ON [dbo].[UsuarioVeiculo]
(
	[VeiculoRefId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
CREATE NONCLUSTERED INDEX [IX_Ocupacao_Id] ON [dbo].[VagaModels]
(
	[Ocupacao_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
CREATE NONCLUSTERED INDEX [IX_PontoModel_Id] ON [dbo].[VagaModels]
(
	[PontoModel_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
CREATE NONCLUSTERED INDEX [IX_Reserva_Id] ON [dbo].[VagaModels]
(
	[Reserva_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
CREATE NONCLUSTERED INDEX [IX_Responsavel_Id] ON [dbo].[VagaModels]
(
	[Responsavel_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
ALTER TABLE [dbo].[EstacionamentoModels] ADD  DEFAULT ((0)) FOR [ImagemAltura]
GO
ALTER TABLE [dbo].[EstacionamentoModels] ADD  DEFAULT ((0)) FOR [ImagemLargura]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[EstacionamentoModels]  WITH CHECK ADD  CONSTRAINT [FK_dbo.EstacionamentoModels_dbo.UsuarioModels_Responsavel_Id] FOREIGN KEY([Responsavel_Id])
REFERENCES [dbo].[UsuarioModels] ([Id])
GO
ALTER TABLE [dbo].[EstacionamentoModels] CHECK CONSTRAINT [FK_dbo.EstacionamentoModels_dbo.UsuarioModels_Responsavel_Id]
GO
ALTER TABLE [dbo].[OcupacaoModels]  WITH CHECK ADD  CONSTRAINT [FK_dbo.OcupacaoModels_dbo.UsuarioModels_Usuario_Id] FOREIGN KEY([Usuario_Id])
REFERENCES [dbo].[UsuarioModels] ([Id])
GO
ALTER TABLE [dbo].[OcupacaoModels] CHECK CONSTRAINT [FK_dbo.OcupacaoModels_dbo.UsuarioModels_Usuario_Id]
GO
ALTER TABLE [dbo].[OcupacaoModels]  WITH CHECK ADD  CONSTRAINT [FK_dbo.OcupacaoModels_dbo.VeiculoModels_Veiculo_Id] FOREIGN KEY([Veiculo_Id])
REFERENCES [dbo].[VeiculoModels] ([Id])
GO
ALTER TABLE [dbo].[OcupacaoModels] CHECK CONSTRAINT [FK_dbo.OcupacaoModels_dbo.VeiculoModels_Veiculo_Id]
GO
ALTER TABLE [dbo].[PontoModels]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PontoModels_dbo.EstacionamentoModels_EstacionamentoModel_Id] FOREIGN KEY([EstacionamentoModel_Id])
REFERENCES [dbo].[EstacionamentoModels] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PontoModels] CHECK CONSTRAINT [FK_dbo.PontoModels_dbo.EstacionamentoModels_EstacionamentoModel_Id]
GO
ALTER TABLE [dbo].[PontoModels]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PontoModels_dbo.PontoModels_PontoModel_Id] FOREIGN KEY([PontoModel_Id])
REFERENCES [dbo].[PontoModels] ([Id])
GO
ALTER TABLE [dbo].[PontoModels] CHECK CONSTRAINT [FK_dbo.PontoModels_dbo.PontoModels_PontoModel_Id]
GO
ALTER TABLE [dbo].[ReservaModels]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ReservaModels_dbo.UsuarioModels_Usuario_Id] FOREIGN KEY([Usuario_Id])
REFERENCES [dbo].[UsuarioModels] ([Id])
GO
ALTER TABLE [dbo].[ReservaModels] CHECK CONSTRAINT [FK_dbo.ReservaModels_dbo.UsuarioModels_Usuario_Id]
GO
ALTER TABLE [dbo].[ReservaModels]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ReservaModels_dbo.VeiculoModels_Veiculo_Id] FOREIGN KEY([Veiculo_Id])
REFERENCES [dbo].[VeiculoModels] ([Id])
GO
ALTER TABLE [dbo].[ReservaModels] CHECK CONSTRAINT [FK_dbo.ReservaModels_dbo.VeiculoModels_Veiculo_Id]
GO
ALTER TABLE [dbo].[UsuarioVeiculo]  WITH CHECK ADD  CONSTRAINT [FK_dbo.UsuarioVeiculo_dbo.UsuarioModels_UsuarioRefId] FOREIGN KEY([UsuarioRefId])
REFERENCES [dbo].[UsuarioModels] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UsuarioVeiculo] CHECK CONSTRAINT [FK_dbo.UsuarioVeiculo_dbo.UsuarioModels_UsuarioRefId]
GO
ALTER TABLE [dbo].[UsuarioVeiculo]  WITH CHECK ADD  CONSTRAINT [FK_dbo.UsuarioVeiculo_dbo.VeiculoModels_VeiculoRefId] FOREIGN KEY([VeiculoRefId])
REFERENCES [dbo].[VeiculoModels] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UsuarioVeiculo] CHECK CONSTRAINT [FK_dbo.UsuarioVeiculo_dbo.VeiculoModels_VeiculoRefId]
GO
ALTER TABLE [dbo].[VagaModels]  WITH CHECK ADD  CONSTRAINT [FK_dbo.VagaModels_dbo.OcupacaoModels_Ocupacao_Id] FOREIGN KEY([Ocupacao_Id])
REFERENCES [dbo].[OcupacaoModels] ([Id])
GO
ALTER TABLE [dbo].[VagaModels] CHECK CONSTRAINT [FK_dbo.VagaModels_dbo.OcupacaoModels_Ocupacao_Id]
GO
ALTER TABLE [dbo].[VagaModels]  WITH CHECK ADD  CONSTRAINT [FK_dbo.VagaModels_dbo.PontoModels_PontoModel_Id] FOREIGN KEY([PontoModel_Id])
REFERENCES [dbo].[PontoModels] ([Id])
GO
ALTER TABLE [dbo].[VagaModels] CHECK CONSTRAINT [FK_dbo.VagaModels_dbo.PontoModels_PontoModel_Id]
GO
ALTER TABLE [dbo].[VagaModels]  WITH CHECK ADD  CONSTRAINT [FK_dbo.VagaModels_dbo.ReservaModels_Reserva_Id] FOREIGN KEY([Reserva_Id])
REFERENCES [dbo].[ReservaModels] ([Id])
GO
ALTER TABLE [dbo].[VagaModels] CHECK CONSTRAINT [FK_dbo.VagaModels_dbo.ReservaModels_Reserva_Id]
GO
ALTER TABLE [dbo].[VagaModels]  WITH CHECK ADD  CONSTRAINT [FK_dbo.VagaModels_dbo.UsuarioModels_Responsavel_Id] FOREIGN KEY([Responsavel_Id])
REFERENCES [dbo].[UsuarioModels] ([Id])
GO
ALTER TABLE [dbo].[VagaModels] CHECK CONSTRAINT [FK_dbo.VagaModels_dbo.UsuarioModels_Responsavel_Id]
GO
