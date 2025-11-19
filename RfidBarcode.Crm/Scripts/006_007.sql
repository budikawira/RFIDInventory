START TRANSACTION;

ALTER TABLE `TrackingItems` MODIFY `Point` longtext NULL;

ALTER TABLE `Items` MODIFY `Point` longtext NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20251119163200_006_Finish', '8.0.8');

COMMIT;

START TRANSACTION;

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

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20251119164751_007_ImportItemLogs', '8.0.8');

COMMIT;

