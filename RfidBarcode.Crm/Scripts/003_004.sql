START TRANSACTION;

INSERT INTO `AccessMenus` (`Id`, `CreatedBy`, `CreatedDate`, `Description`, `LastUpdateBy`, `LastUpdateDate`)
VALUES ('RM', 'system', '2025-12-30 00:00:00.000000', 'Role Management', 'system', '2025-12-30 00:00:00.000000');

INSERT INTO `AccessMenuRoles` (`Id`, `AccessMenuId`, `CreatedBy`, `CreatedDate`, `LastUpdateBy`, `LastUpdateDate`, `RoleId`)
VALUES (2, 'RM', 'system', '2025-12-30 00:00:00.000000', 'system', '2025-12-30 00:00:00.000000', 1);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20251230024839_003_AccessMenu', '8.0.8');

COMMIT;

START TRANSACTION;

DELETE FROM `AspNetRoles`
WHERE `Id` = 5;

DELETE FROM `AspNetRoles`
WHERE `Id` = 6;

DELETE FROM `AspNetRoles`
WHERE `Id` = 7;

DELETE FROM `AspNetRoles`
WHERE `Id` = 8;

INSERT INTO `AccessMenus` (`Id`, `CreatedBy`, `CreatedDate`, `Description`, `LastUpdateBy`, `LastUpdateDate`)
VALUES ('IB', 'system', '2025-12-30 00:00:00.000000', 'Input Barcode', 'system', '2025-12-30 00:00:00.000000');
INSERT INTO `AccessMenus` (`Id`, `CreatedBy`, `CreatedDate`, `Description`, `LastUpdateBy`, `LastUpdateDate`)
VALUES ('SJI', 'system', '2025-12-30 00:00:00.000000', 'Surat Jalan Inbound', 'system', '2025-12-30 00:00:00.000000');
INSERT INTO `AccessMenus` (`Id`, `CreatedBy`, `CreatedDate`, `Description`, `LastUpdateBy`, `LastUpdateDate`)
VALUES ('SJO', 'system', '2025-12-30 00:00:00.000000', 'Surat Jalan Outbond', 'system', '2025-12-30 00:00:00.000000');

UPDATE `AspNetRoles` SET `Name` = 'Adm Barcode', `NormalizedName` = 'ADM BARCODE'
WHERE `Id` = 2;

UPDATE `AspNetRoles` SET `Name` = 'Adm Finish', `NormalizedName` = 'ADM FINISH'
WHERE `Id` = 3;

UPDATE `AspNetRoles` SET `Name` = 'Adm Gudang', `NormalizedName` = 'ADM GUDANG'
WHERE `Id` = 4;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20251230072830_004_Role', '8.0.8');

COMMIT;