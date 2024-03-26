﻿// <auto-generated />
using System;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240326063148_PushTokenNullable")]
    partial class PushTokenNullable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AppleDeviceAppleWalletPass", b =>
                {
                    b.Property<string>("AppleDevicesId")
                        .HasColumnType("text")
                        .HasColumnName("apple_devices_id");

                    b.Property<string>("AppleWalletPassesPassId")
                        .HasColumnType("text")
                        .HasColumnName("apple_wallet_passes_pass_id");

                    b.HasKey("AppleDevicesId", "AppleWalletPassesPassId")
                        .HasName("pk_apple_device_apple_wallet_pass");

                    b.HasIndex("AppleWalletPassesPassId")
                        .HasDatabaseName("ix_apple_device_apple_wallet_pass_apple_wallet_passes_pass_id");

                    b.ToTable("apple_device_apple_wallet_pass", (string)null);
                });

            modelBuilder.Entity("BL.Entities.AppleAssociatedStoreApp", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("AppleWalletPartnerSpecificId")
                        .HasColumnType("bigint")
                        .HasColumnName("apple_wallet_partner_specific_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_apple_associated_store_apps");

                    b.HasIndex("AppleWalletPartnerSpecificId")
                        .HasDatabaseName("ix_apple_associated_store_apps_apple_wallet_partner_specific_id");

                    b.ToTable("apple_associated_store_apps", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            AppleWalletPartnerSpecificId = 1L,
                            Name = "Интенс APP"
                        });
                });

            modelBuilder.Entity("BL.Entities.AppleDevice", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.HasKey("Id")
                        .HasName("pk_apple_devices");

                    b.ToTable("apple_devices", (string)null);
                });

            modelBuilder.Entity("BL.Entities.AppleWalletPartnerSpecific", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("BackgroundColor")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("background_color");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("IconPath")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("icon_path");

                    b.Property<string>("LogoPath")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("logo_path");

                    b.Property<long>("PartnerId")
                        .HasColumnType("bigint")
                        .HasColumnName("partner_id");

                    b.Property<string>("StripPath")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("strip_path");

                    b.HasKey("Id")
                        .HasName("pk_apple_wallet_partner_specifics");

                    b.HasIndex("PartnerId")
                        .IsUnique()
                        .HasDatabaseName("ix_apple_wallet_partner_specifics_partner_id");

                    b.ToTable("apple_wallet_partner_specifics", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            BackgroundColor = "#5bd1e1",
                            Description = "Интенс APP",
                            IconPath = "Intens APP Icon 1x.png",
                            LogoPath = "Intens APP Icon 1x.png",
                            PartnerId = 1L,
                            StripPath = "Intens.png"
                        });
                });

            modelBuilder.Entity("BL.Entities.AppleWalletPass", b =>
                {
                    b.Property<string>("PassId")
                        .HasColumnType("text")
                        .HasColumnName("pass_id");

                    b.Property<long>("CardId")
                        .HasColumnType("bigint")
                        .HasColumnName("card_id");

                    b.Property<DateTimeOffset>("LastUpdated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_updated");

                    b.Property<string>("PushToken")
                        .HasColumnType("text")
                        .HasColumnName("push_token");

                    b.HasKey("PassId")
                        .HasName("pk_apple_wallet_passes");

                    b.HasIndex("CardId")
                        .IsUnique()
                        .HasDatabaseName("ix_apple_wallet_passes_card_id");

                    b.ToTable("apple_wallet_passes", (string)null);
                });

            modelBuilder.Entity("BL.Entities.Card", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("ParticipantId")
                        .HasColumnType("bigint")
                        .HasColumnName("participant_id");

                    b.Property<long>("PartnerId")
                        .HasColumnType("bigint")
                        .HasColumnName("partner_id");

                    b.Property<string>("PassId")
                        .HasColumnType("text")
                        .HasColumnName("pass_id");

                    b.Property<string>("UserHashId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_hash_id");

                    b.HasKey("Id")
                        .HasName("pk_cards");

                    b.HasIndex("ParticipantId")
                        .IsUnique()
                        .HasDatabaseName("ix_cards_participant_id");

                    b.HasIndex("PartnerId")
                        .HasDatabaseName("ix_cards_partner_id");

                    b.ToTable("cards", (string)null);
                });

            modelBuilder.Entity("BL.Entities.Partner", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<long>("PartnerSpecificId")
                        .HasColumnType("bigint")
                        .HasColumnName("partner_specific_id");

                    b.HasKey("Id")
                        .HasName("pk_partners");

                    b.ToTable("partners", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Name = "Лукоил",
                            PartnerSpecificId = 0L
                        });
                });

            modelBuilder.Entity("BL.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("login");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("role");

                    b.HasKey("Id");

                    b.HasAlternateKey("Login")
                        .HasName("ak_users_login");

                    b.ToTable("users", (string)null);

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("BL.Entities.Participant", b =>
                {
                    b.HasBaseType("BL.Entities.User");

                    b.Property<decimal>("Balance")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric(2)")
                        .HasDefaultValue(0.00m)
                        .HasColumnName("balance");

                    b.Property<long>("CardId")
                        .HasColumnType("bigint")
                        .HasColumnName("card_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.ToTable("participants", (string)null);
                });

            modelBuilder.Entity("AppleDeviceAppleWalletPass", b =>
                {
                    b.HasOne("BL.Entities.AppleDevice", null)
                        .WithMany()
                        .HasForeignKey("AppleDevicesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_apple_device_apple_wallet_pass_apple_devices_apple_devices_");

                    b.HasOne("BL.Entities.AppleWalletPass", null)
                        .WithMany()
                        .HasForeignKey("AppleWalletPassesPassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_apple_device_apple_wallet_pass_apple_wallet_passes_apple_wa");
                });

            modelBuilder.Entity("BL.Entities.AppleAssociatedStoreApp", b =>
                {
                    b.HasOne("BL.Entities.AppleWalletPartnerSpecific", "AppleWalletPartnerSpecific")
                        .WithMany("AppleAssociatedStoreApps")
                        .HasForeignKey("AppleWalletPartnerSpecificId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_apple_associated_store_apps_apple_wallet_partner_specifics_");

                    b.Navigation("AppleWalletPartnerSpecific");
                });

            modelBuilder.Entity("BL.Entities.AppleWalletPartnerSpecific", b =>
                {
                    b.HasOne("BL.Entities.Partner", "Partner")
                        .WithOne("PartnerSpecific")
                        .HasForeignKey("BL.Entities.AppleWalletPartnerSpecific", "PartnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_apple_wallet_partner_specifics_partners_partner_id");

                    b.Navigation("Partner");
                });

            modelBuilder.Entity("BL.Entities.AppleWalletPass", b =>
                {
                    b.HasOne("BL.Entities.Card", "Card")
                        .WithOne("AppleWalletPass")
                        .HasForeignKey("BL.Entities.AppleWalletPass", "CardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_apple_wallet_passes_cards_card_id");

                    b.Navigation("Card");
                });

            modelBuilder.Entity("BL.Entities.Card", b =>
                {
                    b.HasOne("BL.Entities.Participant", "Participant")
                        .WithOne("Card")
                        .HasForeignKey("BL.Entities.Card", "ParticipantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_cards_participants_participant_id");

                    b.HasOne("BL.Entities.Partner", "Partner")
                        .WithMany("Cards")
                        .HasForeignKey("PartnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_cards_partners_partner_id");

                    b.Navigation("Participant");

                    b.Navigation("Partner");
                });

            modelBuilder.Entity("BL.Entities.Participant", b =>
                {
                    b.HasOne("BL.Entities.User", null)
                        .WithOne()
                        .HasForeignKey("BL.Entities.Participant", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_participants_users_id");
                });

            modelBuilder.Entity("BL.Entities.AppleWalletPartnerSpecific", b =>
                {
                    b.Navigation("AppleAssociatedStoreApps");
                });

            modelBuilder.Entity("BL.Entities.Card", b =>
                {
                    b.Navigation("AppleWalletPass");
                });

            modelBuilder.Entity("BL.Entities.Partner", b =>
                {
                    b.Navigation("Cards");

                    b.Navigation("PartnerSpecific");
                });

            modelBuilder.Entity("BL.Entities.Participant", b =>
                {
                    b.Navigation("Card");
                });
#pragma warning restore 612, 618
        }
    }
}
