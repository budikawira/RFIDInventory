START TRANSACTION;

ALTER TABLE `Items` ADD `LocationId` bigint NULL;

ALTER TABLE `Items` ADD `Qr` longtext NULL;

CREATE TABLE `Gates` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Name` longtext NOT NULL,
    `ClientId` longtext NOT NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `Locations` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Name` longtext NOT NULL,
    `Description` longtext NULL,
    `Type` tinyint unsigned NOT NULL,
    `SkipStockOpname` int NOT NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `GateMaps` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `GateId` bigint NOT NULL,
    `Antenna` longtext NULL,
    `PrevLocationId` bigint NULL,
    `NextLocationId` bigint NOT NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_GateMaps_Gates_GateId` FOREIGN KEY (`GateId`) REFERENCES `Gates` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_GateMaps_Locations_NextLocationId` FOREIGN KEY (`NextLocationId`) REFERENCES `Locations` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_GateMaps_Locations_PrevLocationId` FOREIGN KEY (`PrevLocationId`) REFERENCES `Locations` (`Id`) ON DELETE CASCADE
);

UPDATE `AccessMenuRoles` SET `CreatedDate` = '2025-03-12 11:14:08.970804', `LastUpdateDate` = '2025-03-12 11:14:08.970804'
WHERE `Id` = 1;

UPDATE `AccessMenus` SET `CreatedDate` = '2025-03-12 11:14:08.970784', `LastUpdateDate` = '2025-03-12 11:14:08.970787'
WHERE `Id` = 'UM';

CREATE INDEX `IX_Items_LocationId` ON `Items` (`LocationId`);

CREATE INDEX `IX_GateMaps_GateId` ON `GateMaps` (`GateId`);

CREATE INDEX `IX_GateMaps_NextLocationId` ON `GateMaps` (`NextLocationId`);

CREATE INDEX `IX_GateMaps_PrevLocationId` ON `GateMaps` (`PrevLocationId`);

ALTER TABLE `Items` ADD CONSTRAINT `FK_Items_Locations_LocationId` FOREIGN KEY (`LocationId`) REFERENCES `Locations` (`Id`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250312041409_003_Location', '8.0.8');

COMMIT;

