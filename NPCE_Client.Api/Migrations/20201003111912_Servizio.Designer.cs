﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NPCE_Client.Api.Data;

namespace NPCE_Client.Api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20201003111912_Servizio")]
    partial class Servizio
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NPCE_Client.Model.Ambiente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ColUri")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Contratto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContrattoCOL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContrattoMOL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsPil")
                        .HasColumnType("bit");

                    b.Property<string>("LolUri")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MolUri")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RolUri")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VolUri")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("billingcenter")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("codicefiscale")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("contractid")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("contracttype")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("costcenter")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("customer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("customerid")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("idsender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("partitaiva")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("sendersystem")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("smuser")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("usertype")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Ambienti");
                });

            modelBuilder.Entity("NPCE_Client.Model.Anagrafica", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Cap")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CasellaPostale")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Citta")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Cognome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ComplementoIndirizzo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ComplementoNominativo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DUG")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Esponente")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Frazione")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NumeroCivico")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Provincia")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RagioneSociale")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Stato")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefono")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Toponimo")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Anagrafiche");
                });

            modelBuilder.Entity("NPCE_Client.Model.Documento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte[]>("Content")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Extension")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.Property<string>("Tag")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Documenti");
                });

            modelBuilder.Entity("NPCE_Client.Model.TipoServizio", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TipiServizio");

                    b.HasData(
                        new
                        {
                            Id = 0,
                            Description = "Posta 1"
                        },
                        new
                        {
                            Id = 1,
                            Description = "Posta 4"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Raccomandata"
                        },
                        new
                        {
                            Id = 3,
                            Description = "PostaContest 1"
                        },
                        new
                        {
                            Id = 4,
                            Description = "PostaContest 4"
                        },
                        new
                        {
                            Id = 5,
                            Description = "RaccomandataMarket 1"
                        },
                        new
                        {
                            Id = 6,
                            Description = "RaccomandataMarket 4"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
