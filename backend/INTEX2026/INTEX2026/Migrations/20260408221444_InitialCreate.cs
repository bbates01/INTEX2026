using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace INTEX2026.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    AppointmentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StaffUserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    ResidentId = table.Column<int>(type: "integer", nullable: false),
                    EventName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    AppointmentDate = table.Column<DateOnly>(type: "date", nullable: false),
                    AppointmentTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    AppointmentType = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    SessionFormat = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Scheduled"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.AppointmentId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PrivacyPolicyAccepted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CookieConsentAccepted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    PrivacyPolicyAcceptedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CookieConsentAcceptedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SafehouseId = table.Column<int>(type: "integer", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DonationAllocations",
                columns: table => new
                {
                    AllocationId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DonationId = table.Column<int>(type: "integer", nullable: false),
                    SafehouseId = table.Column<int>(type: "integer", nullable: false),
                    ProgramArea = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AmountAllocated = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    AllocationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    AllocationNotes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonationAllocations", x => x.AllocationId);
                });

            migrationBuilder.CreateTable(
                name: "Donations",
                columns: table => new
                {
                    DonationId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SupporterId = table.Column<int>(type: "integer", nullable: false),
                    DonationType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DonationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    IsRecurring = table.Column<bool>(type: "boolean", nullable: false),
                    CampaignName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    ChannelSource = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CurrencyCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValue: "PHP"),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    EstimatedValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    ImpactUnit = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    ReferralPostId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donations", x => x.DonationId);
                });

            migrationBuilder.CreateTable(
                name: "EducationRecords",
                columns: table => new
                {
                    EducationRecordId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ResidentId = table.Column<int>(type: "integer", nullable: false),
                    RecordDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ProgramName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CourseName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    EducationLevel = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SchoolName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    EnrollmentStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AttendanceRate = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    ProgressPercent = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    CompletionStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    GpaLikeScore = table.Column<decimal>(type: "numeric(3,2)", precision: 3, scale: 2, nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationRecords", x => x.EducationRecordId);
                });

            migrationBuilder.CreateTable(
                name: "HealthWellbeingRecords",
                columns: table => new
                {
                    HealthRecordId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ResidentId = table.Column<int>(type: "integer", nullable: false),
                    RecordDate = table.Column<DateOnly>(type: "date", nullable: false),
                    GeneralHealthScore = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    NutritionScore = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    SleepQualityScore = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    EnergyLevelScore = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    HeightCm = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false),
                    WeightKg = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false),
                    Bmi = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    MedicalCheckupDone = table.Column<bool>(type: "boolean", nullable: false),
                    DentalCheckupDone = table.Column<bool>(type: "boolean", nullable: false),
                    PsychologicalCheckupDone = table.Column<bool>(type: "boolean", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthWellbeingRecords", x => x.HealthRecordId);
                });

            migrationBuilder.CreateTable(
                name: "HomeVisitations",
                columns: table => new
                {
                    VisitationId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ResidentId = table.Column<int>(type: "integer", nullable: false),
                    VisitDate = table.Column<DateOnly>(type: "date", nullable: false),
                    SocialWorker = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    VisitType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LocationVisited = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    FamilyMembersPresent = table.Column<string>(type: "text", nullable: false),
                    Purpose = table.Column<string>(type: "text", nullable: false),
                    Observations = table.Column<string>(type: "text", nullable: false),
                    FamilyCooperationLevel = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    SafetyConcernsNoted = table.Column<bool>(type: "boolean", nullable: false),
                    FollowUpNeeded = table.Column<bool>(type: "boolean", nullable: false),
                    FollowUpNotes = table.Column<string>(type: "text", nullable: true),
                    VisitOutcome = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeVisitations", x => x.VisitationId);
                });

            migrationBuilder.CreateTable(
                name: "IncidentReports",
                columns: table => new
                {
                    IncidentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ResidentId = table.Column<int>(type: "integer", nullable: false),
                    SafehouseId = table.Column<int>(type: "integer", nullable: false),
                    IncidentDate = table.Column<DateOnly>(type: "date", nullable: false),
                    IncidentType = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Severity = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Low"),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ResponseTaken = table.Column<string>(type: "text", nullable: false),
                    Resolved = table.Column<bool>(type: "boolean", nullable: false),
                    ResolutionDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ReportedBy = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    FollowUpRequired = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncidentReports", x => x.IncidentId);
                });

            migrationBuilder.CreateTable(
                name: "InKindDonationItems",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DonationId = table.Column<int>(type: "integer", nullable: false),
                    ItemName = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    ItemCategory = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    UnitOfMeasure = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    EstimatedUnitValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    IntendedUse = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ReceivedCondition = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InKindDonationItems", x => x.ItemId);
                });

            migrationBuilder.CreateTable(
                name: "InterventionPlans",
                columns: table => new
                {
                    PlanId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ResidentId = table.Column<int>(type: "integer", nullable: false),
                    PlanCategory = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PlanDescription = table.Column<string>(type: "text", nullable: false),
                    ServicesProvided = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TargetValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    TargetDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false, defaultValue: "Open"),
                    CaseConferenceDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterventionPlans", x => x.PlanId);
                });

            migrationBuilder.CreateTable(
                name: "PartnerAssignments",
                columns: table => new
                {
                    AssignmentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PartnerId = table.Column<int>(type: "integer", nullable: false),
                    SafehouseId = table.Column<int>(type: "integer", nullable: false),
                    ProgramArea = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AssignmentStart = table.Column<DateOnly>(type: "date", nullable: true),
                    AssignmentEnd = table.Column<DateOnly>(type: "date", nullable: true),
                    ResponsibilityNotes = table.Column<string>(type: "text", nullable: true),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Active")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnerAssignments", x => x.AssignmentId);
                });

            migrationBuilder.CreateTable(
                name: "Partners",
                columns: table => new
                {
                    PartnerId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PartnerName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    PartnerType = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    RoleType = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    ContactName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Region = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Active"),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partners", x => x.PartnerId);
                });

            migrationBuilder.CreateTable(
                name: "ProcessRecordings",
                columns: table => new
                {
                    RecordingId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ResidentId = table.Column<int>(type: "integer", nullable: false),
                    SessionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    SocialWorker = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    SessionType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    SessionDurationMinutes = table.Column<int>(type: "integer", nullable: false),
                    EmotionalStateObserved = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    EmotionalStateEnd = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    SessionNarrative = table.Column<string>(type: "text", nullable: false),
                    InterventionsApplied = table.Column<string>(type: "text", nullable: false),
                    FollowUpActions = table.Column<string>(type: "text", nullable: false),
                    ProgressNoted = table.Column<bool>(type: "boolean", nullable: false),
                    ConcernsFlagged = table.Column<bool>(type: "boolean", nullable: false),
                    ReferralMade = table.Column<bool>(type: "boolean", nullable: false),
                    NotesRestricted = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessRecordings", x => x.RecordingId);
                });

            migrationBuilder.CreateTable(
                name: "PublicImpactSnapshots",
                columns: table => new
                {
                    SnapshotId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SnapshotDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Headline = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SummaryText = table.Column<string>(type: "text", nullable: false),
                    MetricPayloadJson = table.Column<string>(type: "text", nullable: false, defaultValue: "{}"),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicImpactSnapshots", x => x.SnapshotId);
                });

            migrationBuilder.CreateTable(
                name: "Residents",
                columns: table => new
                {
                    ResidentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CaseControlNo = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    InternalCode = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    SafehouseId = table.Column<int>(type: "integer", nullable: false),
                    CaseStatus = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, defaultValue: "Active"),
                    Sex = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValue: "F"),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    BirthStatus = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    PlaceOfBirth = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Religion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CaseCategory = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    SubCategories = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    PersonWithDisability = table.Column<bool>(type: "boolean", nullable: false),
                    PwdType = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    HasSpecialNeeds = table.Column<bool>(type: "boolean", nullable: false),
                    SpecialNeedsDiagnosis = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Is4PsBeneficiary = table.Column<bool>(type: "boolean", nullable: false),
                    SoloParentHousehold = table.Column<bool>(type: "boolean", nullable: false),
                    IndigenousFamily = table.Column<bool>(type: "boolean", nullable: false),
                    ParentIsPwd = table.Column<bool>(type: "boolean", nullable: false),
                    InformalSettler = table.Column<bool>(type: "boolean", nullable: false),
                    AssignedSocialWorker = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ReferralSource = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    ReferringAgency = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    DateColbRegistered = table.Column<DateOnly>(type: "date", nullable: true),
                    DateColbObtained = table.Column<DateOnly>(type: "date", nullable: true),
                    InitialCaseAssessment = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    DateCaseStudyPrepared = table.Column<DateOnly>(type: "date", nullable: true),
                    ReintegrationType = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, defaultValue: "None"),
                    ReintegrationStatus = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, defaultValue: "Not Started"),
                    InitialRiskLevel = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, defaultValue: "Medium"),
                    CurrentRiskLevel = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, defaultValue: "Medium"),
                    DateOfAdmission = table.Column<DateOnly>(type: "date", nullable: true),
                    DateEnrolled = table.Column<DateOnly>(type: "date", nullable: true),
                    DateClosed = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    InitialNotes = table.Column<string>(type: "text", nullable: true),
                    NotesRestricted = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Residents", x => x.ResidentId);
                });

            migrationBuilder.CreateTable(
                name: "SafehouseMonthlyMetrics",
                columns: table => new
                {
                    MetricId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SafehouseId = table.Column<int>(type: "integer", nullable: false),
                    MonthStart = table.Column<DateOnly>(type: "date", nullable: false),
                    MonthEnd = table.Column<DateOnly>(type: "date", nullable: false),
                    ActiveResidents = table.Column<int>(type: "integer", nullable: false),
                    AvgEducationProgress = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    AvgHealthScore = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    ProcessRecordingCount = table.Column<int>(type: "integer", nullable: false),
                    HomeVisitationCount = table.Column<int>(type: "integer", nullable: false),
                    IncidentCount = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafehouseMonthlyMetrics", x => x.MetricId);
                });

            migrationBuilder.CreateTable(
                name: "Safehouses",
                columns: table => new
                {
                    SafehouseId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SafehouseCode = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Region = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Province = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValue: "Philippines"),
                    OpenDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, defaultValue: "Active"),
                    CapacityGirls = table.Column<int>(type: "integer", nullable: false),
                    CapacityStaff = table.Column<int>(type: "integer", nullable: false),
                    CurrentOccupancy = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Safehouses", x => x.SafehouseId);
                });

            migrationBuilder.CreateTable(
                name: "SocialMediaPosts",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Platform = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PlatformPostId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PostUrl = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CampaignName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Impressions = table.Column<int>(type: "integer", nullable: false),
                    Reach = table.Column<int>(type: "integer", nullable: false),
                    Likes = table.Column<int>(type: "integer", nullable: false),
                    Comments = table.Column<int>(type: "integer", nullable: false),
                    Shares = table.Column<int>(type: "integer", nullable: false),
                    ClickThroughs = table.Column<int>(type: "integer", nullable: false),
                    DonationReferrals = table.Column<int>(type: "integer", nullable: false),
                    EstimatedDonationValuePhp = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialMediaPosts", x => x.PostId);
                });

            migrationBuilder.CreateTable(
                name: "SocialWorkers",
                columns: table => new
                {
                    SocialWorkerId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorkerCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialWorkers", x => x.SocialWorkerId);
                });

            migrationBuilder.CreateTable(
                name: "SocialWorkerUsers",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    SocialWorkerId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialWorkerUsers", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Supporters",
                columns: table => new
                {
                    SupporterId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SupporterType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    OrganizationName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    FirstName = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    LastName = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    RelationshipType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Region = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Active"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    FirstDonationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    AcquisitionChannel = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supporters", x => x.SupporterId);
                });

            migrationBuilder.CreateTable(
                name: "TodoItems",
                columns: table => new
                {
                    TodoId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    TaskText = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoItems", x => x.TodoId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "DonationAllocations");

            migrationBuilder.DropTable(
                name: "Donations");

            migrationBuilder.DropTable(
                name: "EducationRecords");

            migrationBuilder.DropTable(
                name: "HealthWellbeingRecords");

            migrationBuilder.DropTable(
                name: "HomeVisitations");

            migrationBuilder.DropTable(
                name: "IncidentReports");

            migrationBuilder.DropTable(
                name: "InKindDonationItems");

            migrationBuilder.DropTable(
                name: "InterventionPlans");

            migrationBuilder.DropTable(
                name: "PartnerAssignments");

            migrationBuilder.DropTable(
                name: "Partners");

            migrationBuilder.DropTable(
                name: "ProcessRecordings");

            migrationBuilder.DropTable(
                name: "PublicImpactSnapshots");

            migrationBuilder.DropTable(
                name: "Residents");

            migrationBuilder.DropTable(
                name: "SafehouseMonthlyMetrics");

            migrationBuilder.DropTable(
                name: "Safehouses");

            migrationBuilder.DropTable(
                name: "SocialMediaPosts");

            migrationBuilder.DropTable(
                name: "SocialWorkers");

            migrationBuilder.DropTable(
                name: "SocialWorkerUsers");

            migrationBuilder.DropTable(
                name: "Supporters");

            migrationBuilder.DropTable(
                name: "TodoItems");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
