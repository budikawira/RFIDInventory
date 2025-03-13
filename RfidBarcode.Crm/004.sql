START TRANSACTION;

ALTER TABLE `Items` DROP CONSTRAINT `FK_Items_Locations_LocationId`;

ALTER TABLE `Locations` ADD `CreatedBy` longtext NOT NULL;

ALTER TABLE `Locations` ADD `CreatedDate` datetime(6) NULL;

ALTER TABLE `Locations` ADD `LastUpdateBy` longtext NOT NULL;

ALTER TABLE `Locations` ADD `LastUpdateDate` datetime(6) NULL;

ALTER TABLE `Gates` ADD `CreatedBy` longtext NOT NULL;

ALTER TABLE `Gates` ADD `CreatedDate` datetime(6) NULL;

ALTER TABLE `Gates` ADD `LastUpdateBy` longtext NOT NULL;

ALTER TABLE `Gates` ADD `LastUpdateDate` datetime(6) NULL;

ALTER TABLE `GateMaps` ADD `CreatedBy` longtext NOT NULL;

ALTER TABLE `GateMaps` ADD `CreatedDate` datetime(6) NULL;

ALTER TABLE `GateMaps` ADD `LastUpdateBy` longtext NOT NULL;

ALTER TABLE `GateMaps` ADD `LastUpdateDate` datetime(6) NULL;

CREATE TABLE `TagLocations` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Epc` longtext NOT NULL,
    `ItemId` bigint NULL,
    `LocationId` bigint NULL,
    `StartScanned` datetime(6) NOT NULL,
    `EndScanned` datetime(6) NULL,
    `LastScanned` datetime(6) NOT NULL,
    `CreatedDate` datetime(6) NULL,
    `CreatedBy` longtext NOT NULL,
    `LastUpdateDate` datetime(6) NULL,
    `LastUpdateBy` longtext NOT NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TagLocations_Items_ItemId` FOREIGN KEY (`ItemId`) REFERENCES `Items` (`Id`),
    CONSTRAINT `FK_TagLocations_Locations_LocationId` FOREIGN KEY (`LocationId`) REFERENCES `Locations` (`Id`)
);

UPDATE `AccessMenuRoles` SET `CreatedDate` = '2025-03-13 13:51:10.433183', `LastUpdateDate` = '2025-03-13 13:51:10.433184'
WHERE `Id` = 1;

UPDATE `AccessMenus` SET `CreatedDate` = '2025-03-13 13:51:10.433140', `LastUpdateDate` = '2025-03-13 13:51:10.433144'
WHERE `Id` = 'UM';

CREATE INDEX `IX_TagLocations_ItemId` ON `TagLocations` (`ItemId`);

CREATE INDEX `IX_TagLocations_LocationId` ON `TagLocations` (`LocationId`);

ALTER TABLE `Items` ADD CONSTRAINT `FK_Items_Locations_LocationId` FOREIGN KEY (`LocationId`) REFERENCES `Locations` (`Id`) ON DELETE SET NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250313065112_004_Location', '8.0.8');

COMMIT;

