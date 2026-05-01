START TRANSACTION;


            CREATE VIEW ItemPrints AS
                SELECT 
                  i.Id,
                  i.Merk,
                  i.Kp,
                  i.Kode,
                  i.Kode1,
                  i.Kode2,
                  i.Kode3,
                  i.Kode4,
                  i.Oz,
                  i.Grade,
                  i.Point,
                  i.Yard,
                  i.Kg,
                  i.Lebar,
                  i.K,
                  i.SusutLusi,
                  i.SerialNumber,
                  i.K3l,
                  i.Inisial,
                  i.UserId,
                  i.R,
                  i.IdentitasBenang,
                  i.QcFinishUserId,
                  i.QcFinish,
                  i.TanggalBuatBarcode,
                  i.InSuratJalanId,
                  i.InScanUserId,
                  i.InScanUser,
                  i.InScan,
                  i.OutSuratJalanId,
                  i.OutScanUserId,
                  i.OutScanUser,
                  i.OutScan,
                  i.TrackingItemId,
                  i.Epc,
                  i.Qr,
                  i.LocationId,
                  i.SuratJalanId,
                  i.CreatedDate,
                  i.CreatedBy,
                  i.LastUpdateDate,
                  i.LastUpdateBy
                FROM Items i
                WHERE NOT EXISTS (SELECT 1 FROM ItemPrintLogs ipl WHERE ipl.ItemId = i.Id)
            

INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion) VALUES ('20260101000000_CreateItemPrintsView', '8.0.0');

COMMIT;

