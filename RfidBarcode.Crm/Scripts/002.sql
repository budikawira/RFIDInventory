START TRANSACTION;

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

CREATE INDEX `IX_ItemMovements_ItemId` ON `ItemMovements` (`ItemId`);

CREATE INDEX `IX_ItemMovements_LocationId` ON `ItemMovements` (`LocationId`);

CREATE INDEX `IX_ItemMovements_PrevLocationId` ON `ItemMovements` (`PrevLocationId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250518144444_002_ItemMovement', '8.0.8');

COMMIT;

