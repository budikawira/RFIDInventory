START TRANSACTION;

ALTER TABLE `SuratJalans` ADD `ConfirmDate` datetime(6) NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20251208130337_002_SuratJalan', '8.0.8');

COMMIT;

