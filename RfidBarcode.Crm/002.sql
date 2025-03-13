START TRANSACTION;

ALTER TABLE `Items` ADD `Epc` longtext NULL;

UPDATE `AccessMenuRoles` SET `CreatedDate` = '2025-03-04 15:03:15.786332', `LastUpdateDate` = '2025-03-04 15:03:15.786332'
WHERE `Id` = 1;

UPDATE `AccessMenus` SET `CreatedDate` = '2025-03-04 15:03:15.786313', `LastUpdateDate` = '2025-03-04 15:03:15.786315'
WHERE `Id` = 'UM';

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250304080316_002_Epc', '8.0.8');

COMMIT;

