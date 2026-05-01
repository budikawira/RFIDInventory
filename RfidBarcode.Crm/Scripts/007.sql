START TRANSACTION;

CREATE TABLE `StockParams` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `c1` varchar(100) NULL,
    `c2` varchar(100) NULL,
    `c3` varchar(100) NULL,
    `c4` varchar(100) NULL,
    `c5` varchar(100) NULL,
    `p1` longtext NULL,
    `p2` longtext NULL,
    `p3` longtext NULL,
    `p4` longtext NULL,
    `p5` longtext NULL,
    `p6` longtext NULL,
    `p7` longtext NULL,
    `p8` longtext NULL,
    `CreatedDate` datetime(6) NULL,
    `CreatedBy` longtext NOT NULL,
    `LastUpdateDate` datetime(6) NULL,
    `LastUpdateBy` longtext NOT NULL,
    PRIMARY KEY (`Id`)
);

CREATE INDEX `IX_StockParam_c1_c2_c3_c4_c5` ON `StockParams` (`c1`, `c2`, `c3`, `c4`, `c5`);


                ALTER TABLE `StockParams` 
                CONVERT TO CHARACTER SET utf8mb4 
                COLLATE utf8mb4_general_ci;
            

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20260501020654_007_StockParam', '8.0.8');

COMMIT;

