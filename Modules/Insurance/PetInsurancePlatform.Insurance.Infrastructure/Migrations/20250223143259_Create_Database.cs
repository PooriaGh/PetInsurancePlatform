using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PetInsurancePlatform.Insurance.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Create_Database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "insurance");

            migrationBuilder.CreateTable(
                name: "insurance_plans",
                schema: "insurance",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    vip = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    price_value = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_insurance_plans", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "owners",
                schema: "insurance",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    national_id = table.Column<long>(type: "bigint", nullable: false),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_owners", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "pet_types",
                schema: "insurance",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    age_range_maximum_value = table.Column<int>(type: "integer", nullable: false),
                    age_range_minimum_value = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pet_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "provinces",
                schema: "insurance",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_provinces", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "insurance_coverage",
                schema: "insurance",
                columns: table => new
                {
                    insurance_plan_id = table.Column<Guid>(type: "uuid", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_insurance_coverage", x => new { x.insurance_plan_id, x.id });
                    table.ForeignKey(
                        name: "fk_insurance_coverage_insurance_plans_insurance_plan_id",
                        column: x => x.insurance_plan_id,
                        principalSchema: "insurance",
                        principalTable: "insurance_plans",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "terms_of_services",
                schema: "insurance",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    text = table.Column<string>(type: "text", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_terms_of_services", x => x.id);
                    table.ForeignKey(
                        name: "fk_terms_of_services_owners_owner_id",
                        column: x => x.owner_id,
                        principalSchema: "insurance",
                        principalTable: "owners",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "cities",
                schema: "insurance",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    province_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cities", x => x.id);
                    table.ForeignKey(
                        name: "fk_cities_provinces_province_id",
                        column: x => x.province_id,
                        principalSchema: "insurance",
                        principalTable: "provinces",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "owner_terms_of_service",
                schema: "insurance",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    accepted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    terms_of_service_id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_owner_terms_of_service", x => x.id);
                    table.ForeignKey(
                        name: "fk_owner_terms_of_service_owners_owner_id",
                        column: x => x.owner_id,
                        principalSchema: "insurance",
                        principalTable: "owners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_owner_terms_of_service_terms_of_services_terms_of_service_id",
                        column: x => x.terms_of_service_id,
                        principalSchema: "insurance",
                        principalTable: "terms_of_services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "pets",
                schema: "insurance",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    breed = table.Column<string>(type: "text", nullable: false),
                    gender = table.Column<int>(type: "integer", nullable: false),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: false),
                    microchip_code = table.Column<string>(type: "text", nullable: false),
                    deleted = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    pet_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    city_id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: true),
                    address_alley = table.Column<string>(type: "text", nullable: true),
                    address_district = table.Column<int>(type: "integer", nullable: false),
                    address_plate_number = table.Column<int>(type: "integer", nullable: false),
                    address_postal_code = table.Column<long>(type: "bigint", nullable: false),
                    address_street = table.Column<string>(type: "text", nullable: false),
                    appearances_capacity = table.Column<int>(type: "integer", nullable: false),
                    front_view_bucket_name = table.Column<string>(type: "text", nullable: false),
                    front_view_content_type = table.Column<string>(type: "text", nullable: false),
                    front_view_key = table.Column<string>(type: "text", nullable: false),
                    front_view_name = table.Column<string>(type: "text", nullable: false),
                    front_view_size_in_bytes = table.Column<long>(type: "bigint", nullable: false),
                    front_view_uploaded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    health_certificate_bucket_name = table.Column<string>(type: "text", nullable: false),
                    health_certificate_content_type = table.Column<string>(type: "text", nullable: false),
                    health_certificate_key = table.Column<string>(type: "text", nullable: false),
                    health_certificate_name = table.Column<string>(type: "text", nullable: false),
                    health_certificate_size_in_bytes = table.Column<long>(type: "bigint", nullable: false),
                    health_certificate_uploaded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    left_side_view_bucket_name = table.Column<string>(type: "text", nullable: false),
                    left_side_view_content_type = table.Column<string>(type: "text", nullable: false),
                    left_side_view_key = table.Column<string>(type: "text", nullable: false),
                    left_side_view_name = table.Column<string>(type: "text", nullable: false),
                    left_side_view_size_in_bytes = table.Column<long>(type: "bigint", nullable: false),
                    left_side_view_uploaded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    price_value = table.Column<long>(type: "bigint", nullable: false),
                    rear_view_bucket_name = table.Column<string>(type: "text", nullable: false),
                    rear_view_content_type = table.Column<string>(type: "text", nullable: false),
                    rear_view_key = table.Column<string>(type: "text", nullable: false),
                    rear_view_name = table.Column<string>(type: "text", nullable: false),
                    rear_view_size_in_bytes = table.Column<long>(type: "bigint", nullable: false),
                    rear_view_uploaded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    right_side_view_bucket_name = table.Column<string>(type: "text", nullable: false),
                    right_side_view_content_type = table.Column<string>(type: "text", nullable: false),
                    right_side_view_key = table.Column<string>(type: "text", nullable: false),
                    right_side_view_name = table.Column<string>(type: "text", nullable: false),
                    right_side_view_size_in_bytes = table.Column<long>(type: "bigint", nullable: false),
                    right_side_view_uploaded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    walking_video_bucket_name = table.Column<string>(type: "text", nullable: false),
                    walking_video_content_type = table.Column<string>(type: "text", nullable: false),
                    walking_video_key = table.Column<string>(type: "text", nullable: false),
                    walking_video_name = table.Column<string>(type: "text", nullable: false),
                    walking_video_size_in_bytes = table.Column<long>(type: "bigint", nullable: false),
                    walking_video_uploaded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pets", x => x.id);
                    table.ForeignKey(
                        name: "fk_pets_cities_city_id",
                        column: x => x.city_id,
                        principalSchema: "insurance",
                        principalTable: "cities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_pets_owners_owner_id",
                        column: x => x.owner_id,
                        principalSchema: "insurance",
                        principalTable: "owners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_pets_pet_types_pet_type_id",
                        column: x => x.pet_type_id,
                        principalSchema: "insurance",
                        principalTable: "pet_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "diseases",
                schema: "insurance",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    accepted = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    pet_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_diseases", x => x.id);
                    table.ForeignKey(
                        name: "fk_diseases_pets_pet_id",
                        column: x => x.pet_id,
                        principalSchema: "insurance",
                        principalTable: "pets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "insurance_policies",
                schema: "insurance",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    paid_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    issued_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    expired_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    pet_id = table.Column<Guid>(type: "uuid", nullable: true),
                    insurance_plan_id = table.Column<Guid>(type: "uuid", nullable: true),
                    payment_details = table.Column<JsonDocument>(type: "jsonb", nullable: true),
                    payment_method = table.Column<int>(type: "integer", nullable: false),
                    payment_reference_number = table.Column<Guid>(type: "uuid", nullable: false),
                    payment_reservation_number = table.Column<long>(type: "bigint", nullable: false),
                    payment_status = table.Column<int>(type: "integer", nullable: false),
                    payment_amount_value = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_insurance_policies", x => x.id);
                    table.ForeignKey(
                        name: "fk_insurance_policies_insurance_plans_insurance_plan_id",
                        column: x => x.insurance_plan_id,
                        principalSchema: "insurance",
                        principalTable: "insurance_plans",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_insurance_policies_pets_pet_id",
                        column: x => x.pet_id,
                        principalSchema: "insurance",
                        principalTable: "pets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "stored_file",
                schema: "insurance",
                columns: table => new
                {
                    pet_id = table.Column<Guid>(type: "uuid", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    key = table.Column<string>(type: "text", nullable: false),
                    content_type = table.Column<string>(type: "text", nullable: false),
                    bucket_name = table.Column<string>(type: "text", nullable: false),
                    size_in_bytes = table.Column<long>(type: "bigint", nullable: false),
                    uploaded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_stored_file", x => new { x.pet_id, x.id });
                    table.ForeignKey(
                        name: "fk_stored_file_pets_pet_id",
                        column: x => x.pet_id,
                        principalSchema: "insurance",
                        principalTable: "pets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pet_type_diseases",
                schema: "insurance",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    pet_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    disease_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pet_type_diseases", x => x.id);
                    table.ForeignKey(
                        name: "fk_pet_type_diseases_diseases_disease_id",
                        column: x => x.disease_id,
                        principalSchema: "insurance",
                        principalTable: "diseases",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_pet_type_diseases_pet_types_pet_type_id",
                        column: x => x.pet_type_id,
                        principalSchema: "insurance",
                        principalTable: "pet_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_cities_province_id",
                schema: "insurance",
                table: "cities",
                column: "province_id");

            migrationBuilder.CreateIndex(
                name: "ix_diseases_pet_id",
                schema: "insurance",
                table: "diseases",
                column: "pet_id");

            migrationBuilder.CreateIndex(
                name: "ix_insurance_policies_insurance_plan_id",
                schema: "insurance",
                table: "insurance_policies",
                column: "insurance_plan_id");

            migrationBuilder.CreateIndex(
                name: "ix_insurance_policies_pet_id",
                schema: "insurance",
                table: "insurance_policies",
                column: "pet_id");

            migrationBuilder.CreateIndex(
                name: "ix_owner_terms_of_service_owner_id",
                schema: "insurance",
                table: "owner_terms_of_service",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "ix_owner_terms_of_service_terms_of_service_id",
                schema: "insurance",
                table: "owner_terms_of_service",
                column: "terms_of_service_id");

            migrationBuilder.CreateIndex(
                name: "ix_pet_type_diseases_disease_id",
                schema: "insurance",
                table: "pet_type_diseases",
                column: "disease_id");

            migrationBuilder.CreateIndex(
                name: "ix_pet_type_diseases_pet_type_id",
                schema: "insurance",
                table: "pet_type_diseases",
                column: "pet_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_pets_city_id",
                schema: "insurance",
                table: "pets",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "ix_pets_owner_id",
                schema: "insurance",
                table: "pets",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "ix_pets_pet_type_id",
                schema: "insurance",
                table: "pets",
                column: "pet_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_terms_of_services_id",
                schema: "insurance",
                table: "terms_of_services",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_terms_of_services_owner_id",
                schema: "insurance",
                table: "terms_of_services",
                column: "owner_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "insurance_coverage",
                schema: "insurance");

            migrationBuilder.DropTable(
                name: "insurance_policies",
                schema: "insurance");

            migrationBuilder.DropTable(
                name: "owner_terms_of_service",
                schema: "insurance");

            migrationBuilder.DropTable(
                name: "pet_type_diseases",
                schema: "insurance");

            migrationBuilder.DropTable(
                name: "stored_file",
                schema: "insurance");

            migrationBuilder.DropTable(
                name: "insurance_plans",
                schema: "insurance");

            migrationBuilder.DropTable(
                name: "terms_of_services",
                schema: "insurance");

            migrationBuilder.DropTable(
                name: "diseases",
                schema: "insurance");

            migrationBuilder.DropTable(
                name: "pets",
                schema: "insurance");

            migrationBuilder.DropTable(
                name: "cities",
                schema: "insurance");

            migrationBuilder.DropTable(
                name: "owners",
                schema: "insurance");

            migrationBuilder.DropTable(
                name: "pet_types",
                schema: "insurance");

            migrationBuilder.DropTable(
                name: "provinces",
                schema: "insurance");
        }
    }
}
