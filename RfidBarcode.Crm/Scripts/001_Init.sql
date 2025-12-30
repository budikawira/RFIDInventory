CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    PRIMARY KEY (`MigrationId`)
);

START TRANSACTION;

CREATE TABLE `AccessMenus` (
    `Id` varchar(255) NOT NULL,
    `Description` longtext NOT NULL,
    `CreatedDate` datetime(6) NULL,
    `CreatedBy` longtext NOT NULL,
    `LastUpdateDate` datetime(6) NULL,
    `LastUpdateBy` longtext NOT NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `AspNetRoles` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Name` varchar(256) NULL,
    `NormalizedName` varchar(256) NULL,
    `ConcurrencyStamp` longtext NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `AspNetUsers` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `UserName` varchar(256) NULL,
    `NormalizedUserName` varchar(256) NULL,
    `Email` varchar(256) NULL,
    `NormalizedEmail` varchar(256) NULL,
    `EmailConfirmed` tinyint(1) NOT NULL,
    `PasswordHash` longtext NULL,
    `SecurityStamp` longtext NULL,
    `ConcurrencyStamp` longtext NULL,
    `PhoneNumber` longtext NULL,
    `PhoneNumberConfirmed` tinyint(1) NOT NULL,
    `TwoFactorEnabled` tinyint(1) NOT NULL,
    `LockoutEnd` datetime NULL,
    `LockoutEnabled` tinyint(1) NOT NULL,
    `AccessFailedCount` int NOT NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `DailyReports` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Content` longblob NOT NULL,
    `CurrentDate` datetime(6) NOT NULL,
    `PreviousDate` datetime(6) NOT NULL,
    `CreatedDate` datetime(6) NULL,
    `CreatedBy` longtext NOT NULL,
    `LastUpdateDate` datetime(6) NULL,
    `LastUpdateBy` longtext NOT NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `Gates` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Name` longtext NOT NULL,
    `ClientId` longtext NOT NULL,
    `CreatedDate` datetime(6) NULL,
    `CreatedBy` longtext NOT NULL,
    `LastUpdateDate` datetime(6) NULL,
    `LastUpdateBy` longtext NOT NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `ImportItemLogs` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Filename` longtext NOT NULL,
    `Metadata` longtext NOT NULL,
    `CreatedDate` datetime(6) NULL,
    `CreatedBy` longtext NOT NULL,
    `LastUpdateDate` datetime(6) NULL,
    `LastUpdateBy` longtext NOT NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `Locations` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Name` longtext NOT NULL,
    `Description` longtext NULL,
    `Type` tinyint unsigned NOT NULL,
    `SkipStockOpname` int NOT NULL,
    `CreatedDate` datetime(6) NULL,
    `CreatedBy` longtext NOT NULL,
    `LastUpdateDate` datetime(6) NULL,
    `LastUpdateBy` longtext NOT NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `Status` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Name` longtext NOT NULL,
    `CreatedDate` datetime(6) NULL,
    `CreatedBy` longtext NOT NULL,
    `LastUpdateDate` datetime(6) NULL,
    `LastUpdateBy` longtext NOT NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `SuratJalanTypes` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Name` varchar(255) NOT NULL,
    `Type` varchar(255) NOT NULL,
    `CreatedDate` datetime(6) NULL,
    `CreatedBy` longtext NOT NULL,
    `LastUpdateDate` datetime(6) NULL,
    `LastUpdateBy` longtext NOT NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `TrackingItems` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Merk` longtext NULL,
    `Kp` longtext NULL,
    `Ib` longtext NULL,
    `Kode` longtext NULL,
    `Kode1` longtext NULL,
    `Kode2` longtext NULL,
    `Kode3` longtext NULL,
    `Kode4` longtext NULL,
    `Oz` longtext NULL,
    `Grade` longtext NULL,
    `Point` longtext NULL,
    `Yard` decimal(18,2) NULL,
    `Kg` decimal(18,2) NULL,
    `Lebar` double NULL,
    `SusutLusi` longtext NULL,
    `SerialNumber` longtext NULL,
    `Inisial` longtext NULL,
    `EncodeTime` datetime(6) NULL,
    `TrolleyId` bigint NULL,
    `MeterWeaving` float NULL,
    `MeterGreige` float NULL,
    `MeterBBSF` float NULL,
    `WeavingMachineNo` longtext NULL,
    `ProductionDate` datetime(6) NOT NULL,
    `FormId` longtext NULL,
    `NoBeamIndigo` longtext NULL,
    `StockOutDate` datetime(6) NULL,
    `ImportTime` datetime(6) NULL,
    `StartProcess` datetime(6) NULL,
    `EndProcess` datetime(6) NULL,
    `CreatedDate` datetime(6) NULL,
    `CreatedBy` longtext NOT NULL,
    `LastUpdateDate` datetime(6) NULL,
    `LastUpdateBy` longtext NOT NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `AccessMenuRoles` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `AccessMenuId` varchar(255) NOT NULL,
    `RoleId` bigint NOT NULL,
    `CreatedDate` datetime(6) NULL,
    `CreatedBy` longtext NOT NULL,
    `LastUpdateDate` datetime(6) NULL,
    `LastUpdateBy` longtext NOT NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AccessMenuRoles_AccessMenus_AccessMenuId` FOREIGN KEY (`AccessMenuId`) REFERENCES `AccessMenus` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AccessMenuRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AspNetRoleClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `RoleId` bigint NOT NULL,
    `ClaimType` longtext NULL,
    `ClaimValue` longtext NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AspNetUserClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` bigint NOT NULL,
    `ClaimType` longtext NULL,
    `ClaimValue` longtext NULL,
    `ApplicationUserId` bigint NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_ApplicationUserId` FOREIGN KEY (`ApplicationUserId`) REFERENCES `AspNetUsers` (`Id`),
    CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AspNetUserLogins` (
    `LoginProvider` varchar(255) NOT NULL,
    `ProviderKey` varchar(255) NOT NULL,
    `ProviderDisplayName` longtext NULL,
    `UserId` bigint NOT NULL,
    `ApplicationUserId` bigint NULL,
    PRIMARY KEY (`LoginProvider`, `ProviderKey`),
    CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_ApplicationUserId` FOREIGN KEY (`ApplicationUserId`) REFERENCES `AspNetUsers` (`Id`),
    CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AspNetUserRoles` (
    `UserId` bigint NOT NULL,
    `RoleId` bigint NOT NULL,
    PRIMARY KEY (`UserId`, `RoleId`),
    CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AspNetUserTokens` (
    `UserId` bigint NOT NULL,
    `LoginProvider` varchar(255) NOT NULL,
    `Name` varchar(255) NOT NULL,
    `Value` longtext NULL,
    `ApplicationUserId` bigint NULL,
    PRIMARY KEY (`UserId`, `LoginProvider`, `Name`),
    CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_ApplicationUserId` FOREIGN KEY (`ApplicationUserId`) REFERENCES `AspNetUsers` (`Id`),
    CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `SuratJalans` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `SuratJalanName` longtext NULL,
    `SuratJalanType` longtext NOT NULL,
    `No` longtext NULL,
    `Kode` longtext NULL,
    `Kode1` longtext NULL,
    `Kode2` longtext NULL,
    `Kode3` longtext NULL,
    `Kode4` longtext NULL,
    `Grade` longtext NULL,
    `UserId` bigint NOT NULL,
    `FinalizeDate` datetime(6) NULL,
    `Sequence` int NOT NULL,
    `CreatedDate` datetime(6) NULL,
    `CreatedBy` longtext NOT NULL,
    `LastUpdateDate` datetime(6) NULL,
    `LastUpdateBy` longtext NOT NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_SuratJalans_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `GateMaps` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `GateId` bigint NOT NULL,
    `Antenna` longtext NULL,
    `PrevLocationId` bigint NULL,
    `NextLocationId` bigint NOT NULL,
    `CreatedDate` datetime(6) NULL,
    `CreatedBy` longtext NOT NULL,
    `LastUpdateDate` datetime(6) NULL,
    `LastUpdateBy` longtext NOT NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_GateMaps_Gates_GateId` FOREIGN KEY (`GateId`) REFERENCES `Gates` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_GateMaps_Locations_NextLocationId` FOREIGN KEY (`NextLocationId`) REFERENCES `Locations` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_GateMaps_Locations_PrevLocationId` FOREIGN KEY (`PrevLocationId`) REFERENCES `Locations` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `StockOpnames` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `LocationId` bigint NULL,
    `FinalLocationName` longtext NULL,
    `TrolleyId` bigint NULL,
    `FinalTrolleyName` longtext NULL,
    `UserId` bigint NULL,
    `CreatedDate` datetime(6) NULL,
    `CreatedBy` longtext NOT NULL,
    `LastUpdateDate` datetime(6) NULL,
    `LastUpdateBy` longtext NOT NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_StockOpnames_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`),
    CONSTRAINT `FK_StockOpnames_Locations_LocationId` FOREIGN KEY (`LocationId`) REFERENCES `Locations` (`Id`)
);

CREATE TABLE `Items` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Merk` longtext NOT NULL,
    `Kp` longtext NOT NULL,
    `Kode` longtext NULL,
    `Kode1` longtext NULL,
    `Kode2` longtext NULL,
    `Kode3` longtext NULL,
    `Kode4` longtext NULL,
    `Oz` longtext NULL,
    `Grade` longtext NULL,
    `Point` longtext NULL,
    `Yard` decimal(18,2) NULL,
    `Kg` decimal(18,2) NULL,
    `Lebar` longtext NULL,
    `K` longtext NULL,
    `SusutLusi` longtext NULL,
    `SerialNumber` longtext NULL,
    `K3l` longtext NULL,
    `Inisial` longtext NULL,
    `UserId` bigint NOT NULL,
    `R` int NULL,
    `IdentitasBenang` longtext NULL,
    `QcFinishUserId` bigint NULL,
    `QcFinish` datetime(6) NULL,
    `TanggalBuatBarcode` datetime(6) NOT NULL,
    `InSuratJalanId` bigint NULL,
    `InScanUserId` bigint NULL,
    `InScanUser` longtext NULL,
    `InScan` datetime(6) NULL,
    `OutSuratJalanId` bigint NULL,
    `OutScanUserId` bigint NULL,
    `OutScanUser` longtext NULL,
    `OutScan` datetime(6) NULL,
    `TrackingItemId` bigint NULL,
    `Epc` longtext NULL,
    `Qr` longtext NULL,
    `LocationId` bigint NULL,
    `SuratJalanId` bigint NULL,
    `CreatedDate` datetime(6) NULL,
    `CreatedBy` longtext NOT NULL,
    `LastUpdateDate` datetime(6) NULL,
    `LastUpdateBy` longtext NOT NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Items_Locations_LocationId` FOREIGN KEY (`LocationId`) REFERENCES `Locations` (`Id`) ON DELETE SET NULL,
    CONSTRAINT `FK_Items_SuratJalans_InSuratJalanId` FOREIGN KEY (`InSuratJalanId`) REFERENCES `SuratJalans` (`Id`) ON DELETE SET NULL,
    CONSTRAINT `FK_Items_SuratJalans_OutSuratJalanId` FOREIGN KEY (`OutSuratJalanId`) REFERENCES `SuratJalans` (`Id`) ON DELETE SET NULL,
    CONSTRAINT `FK_Items_SuratJalans_SuratJalanId` FOREIGN KEY (`SuratJalanId`) REFERENCES `SuratJalans` (`Id`),
    CONSTRAINT `FK_Items_TrackingItems_TrackingItemId` FOREIGN KEY (`TrackingItemId`) REFERENCES `TrackingItems` (`Id`) ON DELETE SET NULL
);

CREATE TABLE `ItemMovements` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `ItemId` bigint NOT NULL,
    `PrevLocationId` bigint NULL,
    `LocationId` bigint NULL,
    `PrevLocationName` longtext NULL,
    `LocationName` longtext NULL,
    `Note` longtext NULL,
    `Source` longtext NULL,
    `TagLocationId` bigint NULL,
    `CreatedDate` datetime(6) NULL,
    `CreatedBy` longtext NOT NULL,
    `LastUpdateDate` datetime(6) NULL,
    `LastUpdateBy` longtext NOT NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_ItemMovements_Items_ItemId` FOREIGN KEY (`ItemId`) REFERENCES `Items` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_ItemMovements_Locations_LocationId` FOREIGN KEY (`LocationId`) REFERENCES `Locations` (`Id`) ON DELETE SET NULL,
    CONSTRAINT `FK_ItemMovements_Locations_PrevLocationId` FOREIGN KEY (`PrevLocationId`) REFERENCES `Locations` (`Id`) ON DELETE SET NULL
);

CREATE TABLE `ItemPrintLogs` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `ItemId` bigint NOT NULL,
    `CreatedDate` datetime(6) NULL,
    `CreatedBy` longtext NOT NULL,
    `LastUpdateDate` datetime(6) NULL,
    `LastUpdateBy` longtext NOT NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_ItemPrintLogs_Items_ItemId` FOREIGN KEY (`ItemId`) REFERENCES `Items` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `TagLocations` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Epc` longtext NOT NULL,
    `ItemId` bigint NULL,
    `LocationId` bigint NULL,
    `StartScanned` datetime(6) NOT NULL,
    `EndScanned` datetime(6) NULL,
    `LastScanned` datetime(6) NOT NULL,
    `FinalLocation` longtext NULL,
    `Note` longtext NULL,
    `Source` longtext NULL,
    `PrevLocationId` bigint NULL,
    `StockOpnameId` bigint NULL,
    `CreatedDate` datetime(6) NULL,
    `CreatedBy` longtext NOT NULL,
    `LastUpdateDate` datetime(6) NULL,
    `LastUpdateBy` longtext NOT NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TagLocations_Items_ItemId` FOREIGN KEY (`ItemId`) REFERENCES `Items` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_TagLocations_Locations_LocationId` FOREIGN KEY (`LocationId`) REFERENCES `Locations` (`Id`),
    CONSTRAINT `FK_TagLocations_Locations_PrevLocationId` FOREIGN KEY (`PrevLocationId`) REFERENCES `Locations` (`Id`),
    CONSTRAINT `FK_TagLocations_StockOpnames_StockOpnameId` FOREIGN KEY (`StockOpnameId`) REFERENCES `StockOpnames` (`Id`)
);

CREATE TABLE `StockOpnameDetails` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `StockOpnameId` bigint NOT NULL,
    `TagId` longtext NOT NULL,
    `ItemId` bigint NULL,
    `TagLocationId` bigint NULL,
    `FinalLocation` longtext NULL,
    `Note` longtext NULL,
    `CreatedDate` datetime(6) NULL,
    `CreatedBy` longtext NOT NULL,
    `LastUpdateDate` datetime(6) NULL,
    `LastUpdateBy` longtext NOT NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_StockOpnameDetails_Items_ItemId` FOREIGN KEY (`ItemId`) REFERENCES `Items` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_StockOpnameDetails_StockOpnames_StockOpnameId` FOREIGN KEY (`StockOpnameId`) REFERENCES `StockOpnames` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_StockOpnameDetails_TagLocations_TagLocationId` FOREIGN KEY (`TagLocationId`) REFERENCES `TagLocations` (`Id`)
);

INSERT INTO `AccessMenus` (`Id`, `CreatedBy`, `CreatedDate`, `Description`, `LastUpdateBy`, `LastUpdateDate`)
VALUES ('UM', 'system', '2025-05-12 00:00:00.000000', 'User Management', 'system', '2025-05-12 00:00:00.000000');

INSERT INTO `AspNetRoles` (`Id`, `ConcurrencyStamp`, `Name`, `NormalizedName`)
VALUES (1, NULL, 'Administrator', 'ADMINISTRATOR');
INSERT INTO `AspNetRoles` (`Id`, `ConcurrencyStamp`, `Name`, `NormalizedName`)
VALUES (2, NULL, 'Admin Finish', 'ADMIN FINISH');
INSERT INTO `AspNetRoles` (`Id`, `ConcurrencyStamp`, `Name`, `NormalizedName`)
VALUES (3, NULL, 'QC Finish', 'QC FINISH');
INSERT INTO `AspNetRoles` (`Id`, `ConcurrencyStamp`, `Name`, `NormalizedName`)
VALUES (4, NULL, 'Gudang Kain', 'GUDANG KAIN');
INSERT INTO `AspNetRoles` (`Id`, `ConcurrencyStamp`, `Name`, `NormalizedName`)
VALUES (5, NULL, 'QC Gudang Kain', 'QC GUDANG KAIN');
INSERT INTO `AspNetRoles` (`Id`, `ConcurrencyStamp`, `Name`, `NormalizedName`)
VALUES (6, NULL, 'Admin Gudang Kain', 'ADMIN GUDANG KAIN');
INSERT INTO `AspNetRoles` (`Id`, `ConcurrencyStamp`, `Name`, `NormalizedName`)
VALUES (7, NULL, 'Gudang Jakarta', 'GUDANG JAKARTA');
INSERT INTO `AspNetRoles` (`Id`, `ConcurrencyStamp`, `Name`, `NormalizedName`)
VALUES (8, NULL, 'Admin Gudang Jakarta', 'ADMIN GUDANG JAKARTA');

INSERT INTO `AccessMenuRoles` (`Id`, `AccessMenuId`, `CreatedBy`, `CreatedDate`, `LastUpdateBy`, `LastUpdateDate`, `RoleId`)
VALUES (1, 'UM', 'system', '2025-05-12 00:00:00.000000', 'system', '2025-05-12 00:00:00.000000', 1);

CREATE INDEX `IX_AccessMenuRoles_AccessMenuId` ON `AccessMenuRoles` (`AccessMenuId`);

CREATE INDEX `IX_AccessMenuRoles_RoleId` ON `AccessMenuRoles` (`RoleId`);

CREATE INDEX `IX_AspNetRoleClaims_RoleId` ON `AspNetRoleClaims` (`RoleId`);

CREATE UNIQUE INDEX `RoleNameIndex` ON `AspNetRoles` (`NormalizedName`);

CREATE INDEX `IX_AspNetUserClaims_ApplicationUserId` ON `AspNetUserClaims` (`ApplicationUserId`);

CREATE INDEX `IX_AspNetUserClaims_UserId` ON `AspNetUserClaims` (`UserId`);

CREATE INDEX `IX_AspNetUserLogins_ApplicationUserId` ON `AspNetUserLogins` (`ApplicationUserId`);

CREATE INDEX `IX_AspNetUserLogins_UserId` ON `AspNetUserLogins` (`UserId`);

CREATE INDEX `IX_AspNetUserRoles_RoleId` ON `AspNetUserRoles` (`RoleId`);

CREATE INDEX `EmailIndex` ON `AspNetUsers` (`NormalizedEmail`);

CREATE UNIQUE INDEX `UserNameIndex` ON `AspNetUsers` (`NormalizedUserName`);

CREATE INDEX `IX_AspNetUserTokens_ApplicationUserId` ON `AspNetUserTokens` (`ApplicationUserId`);

CREATE INDEX `IX_GateMaps_GateId` ON `GateMaps` (`GateId`);

CREATE INDEX `IX_GateMaps_NextLocationId` ON `GateMaps` (`NextLocationId`);

CREATE INDEX `IX_GateMaps_PrevLocationId` ON `GateMaps` (`PrevLocationId`);

CREATE INDEX `IX_ItemMovements_ItemId` ON `ItemMovements` (`ItemId`);

CREATE INDEX `IX_ItemMovements_LocationId` ON `ItemMovements` (`LocationId`);

CREATE INDEX `IX_ItemMovements_PrevLocationId` ON `ItemMovements` (`PrevLocationId`);

CREATE INDEX `IX_ItemPrintLogs_ItemId` ON `ItemPrintLogs` (`ItemId`);

CREATE INDEX `IX_Items_InSuratJalanId` ON `Items` (`InSuratJalanId`);

CREATE INDEX `IX_Items_LocationId` ON `Items` (`LocationId`);

CREATE INDEX `IX_Items_OutSuratJalanId` ON `Items` (`OutSuratJalanId`);

CREATE INDEX `IX_Items_SuratJalanId` ON `Items` (`SuratJalanId`);

CREATE INDEX `IX_Items_TrackingItemId` ON `Items` (`TrackingItemId`);

CREATE INDEX `IX_StockOpnameDetails_ItemId` ON `StockOpnameDetails` (`ItemId`);

CREATE INDEX `IX_StockOpnameDetails_StockOpnameId` ON `StockOpnameDetails` (`StockOpnameId`);

CREATE INDEX `IX_StockOpnameDetails_TagLocationId` ON `StockOpnameDetails` (`TagLocationId`);

CREATE INDEX `IX_StockOpnames_LocationId` ON `StockOpnames` (`LocationId`);

CREATE INDEX `IX_StockOpnames_UserId` ON `StockOpnames` (`UserId`);

CREATE INDEX `IX_SuratJalans_UserId` ON `SuratJalans` (`UserId`);

CREATE UNIQUE INDEX `IX_SuratJalanTypes_Name` ON `SuratJalanTypes` (`Name`);

CREATE INDEX `IX_SuratJalanTypes_Type` ON `SuratJalanTypes` (`Type`);

CREATE INDEX `IX_TagLocations_ItemId` ON `TagLocations` (`ItemId`);

CREATE INDEX `IX_TagLocations_LocationId` ON `TagLocations` (`LocationId`);

CREATE INDEX `IX_TagLocations_PrevLocationId` ON `TagLocations` (`PrevLocationId`);

CREATE INDEX `IX_TagLocations_StockOpnameId` ON `TagLocations` (`StockOpnameId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20251208091036_001_Init', '8.0.8');

COMMIT;

