START TRANSACTION;

ALTER TABLE `SuratJalans` ADD `IsReturn` tinyint(1) NOT NULL DEFAULT FALSE;

INSERT INTO `AccessMenus` (`Id`, `CreatedBy`, `CreatedDate`, `Description`, `LastUpdateBy`, `LastUpdateDate`)
VALUES ('CI', 'system', '2025-12-30 00:00:00.000000', 'Buat Surat Jalan Inbound', 'system', '2025-12-30 00:00:00.000000');
INSERT INTO `AccessMenus` (`Id`, `CreatedBy`, `CreatedDate`, `Description`, `LastUpdateBy`, `LastUpdateDate`)
VALUES ('CO', 'system', '2025-12-30 00:00:00.000000', 'Buat Surat Jalan Outbond', 'system', '2025-12-30 00:00:00.000000');
INSERT INTO `AccessMenus` (`Id`, `CreatedBy`, `CreatedDate`, `Description`, `LastUpdateBy`, `LastUpdateDate`)
VALUES ('FI', 'system', '2025-12-30 00:00:00.000000', 'Konfirmasi Surat Jalan Inbound', 'system', '2025-12-30 00:00:00.000000');
INSERT INTO `AccessMenus` (`Id`, `CreatedBy`, `CreatedDate`, `Description`, `LastUpdateBy`, `LastUpdateDate`)
VALUES ('FO', 'system', '2025-12-30 00:00:00.000000', 'Konfirmasi Surat Jalan Outbond', 'system', '2025-12-30 00:00:00.000000');
INSERT INTO `AccessMenus` (`Id`, `CreatedBy`, `CreatedDate`, `Description`, `LastUpdateBy`, `LastUpdateDate`)
VALUES ('FOR', 'system', '2025-12-30 00:00:00.000000', 'Konfirmasi Surat Jalan Outbond Retur', 'system', '2025-12-30 00:00:00.000000');

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20251230142701_005_Role', '8.0.8');

COMMIT;