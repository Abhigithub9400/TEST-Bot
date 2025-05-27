
IF NOT EXISTS (
    SELECT *
    FROM Master_Plans 
    WHERE Id in (1,2,3,4)
)
BEGIN
SET IDENTITY_INSERT Master_Plans on;

INSERT INTO Master_Plans (Id, PlanTitle, IsActive,CreatedDate,createdby)
VALUES 
(1, 'Free Plan', 1,GETDATE(),'system'),
(2, 'Pro Plan', 1,GETDATE(),'system'),
(3, 'Advanced Plan', 1,GETDATE(),'system'),
(4, 'Try For Free', 1,GETDATE(),'system');

-- Turn Identity Insert OFF
SET IDENTITY_INSERT Master_Plans OFF;
END

IF NOT EXISTS (
    SELECT *
    FROM Master_Features 
    WHERE Id in (1,2,3,4,5,6,7,8,9)
)
BEGIN
SET IDENTITY_INSERT Master_Features on;
INSERT INTO Master_Features (
	id,
    FeatureName,
    IsActive,
    CreatedDate,
    CreatedBy,
    ModifiedDate,
    ModifiedBy,
	Value
) 
VALUES 
(1,'Transcriptions',1, GETDATE(),'Admin',NULL,NULL,''),
(2,'AvailableHours',1, GETDATE(),'Admin',NULL,NULL,''),
(3,'SessionDurationLimit',1, GETDATE(),'Admin',NULL,NULL,''),
(4,'RealtimeResults',1, GETDATE(),'Admin',NULL,NULL,''),
(5,'PriorityAccessToTheLatestModels',1, GETDATE(),'Admin',NULL,NULL,''),
(6,'EarlyAccessToNewAIFeatures',1, GETDATE(),'Admin',NULL,NULL,''),
(7,'GeneratedocumentsWithConfidence',1, GETDATE(),'Admin',NULL,NULL,''),
(8,'WatermarkRemoval',1, GETDATE(),'Admin',NULL,NULL,''),
(9,'TailoredcapabilitiesAndAdvancedsupport',1, GETDATE(),'Admin',NULL,NULL,'');

SET IDENTITY_INSERT Master_Features OFF;
END 

IF NOT EXISTS (
    SELECT *
    FROM FeaturePlanConfiguration 
    WHERE Id in (1,2,3,4,5,6,7,8,9)
)
BEGIN

SET IDENTITY_INSERT FeaturePlanConfiguration on;
INSERT INTO FeaturePlanConfiguration (Id, planId,FeatureId,IsActive,Value,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy)

VALUES 
(1,1,1,1, '4', GETDATE(), 'Admin', NULL, NULL),
(2,1,2,1, '30', GETDATE(), 'Admin', NULL, NULL),
(3,1,3,1, '10', GETDATE(), 'Admin', NULL, NULL),
(4,1,4,1, '', GETDATE(), 'Admin', NULL, NULL),
(5,1,5,0, '', GETDATE(), 'Admin', NULL, NULL),
(6,1,6,0, '', GETDATE(), 'Admin', NULL, NULL),
(7,1,7,1, '', GETDATE(), 'Admin', NULL, NULL),
(8,1,8,0, '', GETDATE(), 'Admin', NULL, NULL),
(9,1,9,0, '', GETDATE(), 'Admin', NULL, NULL);

SET IDENTITY_INSERT FeaturePlanConfiguration OFF;
END


IF NOT EXISTS (
    SELECT *
    FROM UserTitles 
    WHERE Id in (1,2,3,4,5,6,7)
)
BEGIN
SET IDENTITY_INSERT UserTitles on;
INSERT INTO UserTitles (Id, Title, Abbreviations)
VALUES
    (1, 'Dr. (Doctor)', 'Dr.'),
    (2, 'Consultant', 'Cons.'),
    (3, 'Resident', 'Res.'),
    (4, 'Attending Physician', 'Att.'),
    (5, 'Senior Consultant', 'Sr. Cons.'),
    (6, 'Chief Surgeon', 'Ch. Surg.'),
    (7, 'Clinical Lead', 'Clin. Lead');
SET IDENTITY_INSERT UserTitles off;
END

IF NOT EXISTS (
    SELECT *
    FROM Genders 
    WHERE Id in (1,2,3,4,5)
)
BEGIN
SET IDENTITY_INSERT Genders ON;

INSERT INTO Genders (Id, Gender)
VALUES
    (1, 'Male'),
    (2, 'Female'),
    (3, 'Transgender'),
    (4, 'Non-binary'),
    (5, 'Prefer Not to Say');

SET IDENTITY_INSERT Genders OFF;
END


IF NOT EXISTS (
    SELECT *
    FROM MedicalCredentials 
    WHERE Id in (1,2,3,4,5,6,7,8,9,10)
)
BEGIN
SET IDENTITY_INSERT MedicalCredentials ON;

INSERT INTO MedicalCredentials (Id, MedicalCredentials)
VALUES 
(1, 'MD (Doctor of Medicine)'),
(2, 'MBBS (Bachelor of Medicine, Bachelor of Surgery)'),
(3, 'DO (Doctor of Osteopathic Medicine)'),
(4, 'BDS (Bachelor of Dental Surgery)'),
(5, 'MCh (Master of Surgery)'),
(6, 'DM (Doctorate of Medicine)'),
(7, 'FRCS (Fellowship of the Royal College of Surgeons)'),
(8, 'FACP (Fellow of the American College of Physicians)'),
(9, 'MS (Master of Surgery)'),
(10, 'DNB (Diplomate of National Board)');

SET IDENTITY_INSERT MedicalCredentials OFF;
END


IF NOT EXISTS (
    SELECT *
    FROM Categories 
    WHERE Id in (1,2,3,4,5)
)
BEGIN
SET IDENTITY_INSERT Categories ON;

INSERT INTO Categories (Id, CategoryName, CreatedDate)
VALUES 
(1, 'Usability', GETDATE()),
(2, 'Performance', GETDATE()),
(3, 'Features', GETDATE()),
(4, 'Bugs', GETDATE()),
(5, 'Other', GETDATE());

SET IDENTITY_INSERT Categories OFF;
END

