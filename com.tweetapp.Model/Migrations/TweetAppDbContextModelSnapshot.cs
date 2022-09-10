﻿// <auto-generated />


#nullable disable

using System;
using com.tweetapp.Model.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace com.tweetapp.Model.Migrations
{
    [DbContext(typeof(TweetAppDbContext))]
    partial class TweetAppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("com.tweetapp.Model.Model.TweetDetails", b =>
                {
                    b.Property<int>("TweetID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TweetID"), 1L, 1);

                    b.Property<string>("TweetData")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime>("TweetTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("TweetID");

                    b.HasIndex("UserId");

                    b.ToTable("Tweets");
                });

            modelBuilder.Entity("com.tweetapp.Model.Model.TweetLikes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("HasLiked")
                        .HasColumnType("bit");

                    b.Property<int>("TweetDetailsTweetID")
                        .HasColumnType("int");

                    b.Property<int>("UserDetailsUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TweetDetailsTweetID");

                    b.HasIndex("UserDetailsUserId");

                    b.ToTable("TweetLikes");
                });

            modelBuilder.Entity("com.tweetapp.Model.Model.TweetReplies", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("TweetId")
                        .HasColumnType("int");

                    b.Property<int>("UserDetailsUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TweetId");

                    b.HasIndex("UserDetailsUserId");

                    b.ToTable("TweetReplies");
                });

            modelBuilder.Entity("com.tweetapp.Model.Model.UserDetails", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"), 1L, 1);

                    b.Property<DateTime>("DOB")
                        .HasColumnType("datetime2");

                    b.Property<string>("EmailId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsLoggedIn")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("profileString")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("UserDetails");
                });

            modelBuilder.Entity("com.tweetapp.Model.Model.TweetDetails", b =>
                {
                    b.HasOne("com.tweetapp.Model.Model.UserDetails", "User")
                        .WithMany("Tweets")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("com.tweetapp.Model.Model.TweetLikes", b =>
                {
                    b.HasOne("com.tweetapp.Model.Model.TweetDetails", "TweetDetails")
                        .WithMany()
                        .HasForeignKey("TweetDetailsTweetID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("com.tweetapp.Model.Model.UserDetails", "UserDetails")
                        .WithMany("TweetsLikedCollection")
                        .HasForeignKey("UserDetailsUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TweetDetails");

                    b.Navigation("UserDetails");
                });

            modelBuilder.Entity("com.tweetapp.Model.Model.TweetReplies", b =>
                {
                    b.HasOne("com.tweetapp.Model.Model.TweetDetails", "Tweet")
                        .WithMany()
                        .HasForeignKey("TweetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("com.tweetapp.Model.Model.UserDetails", "UserDetails")
                        .WithMany("RepliesCollection")
                        .HasForeignKey("UserDetailsUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tweet");

                    b.Navigation("UserDetails");
                });

            modelBuilder.Entity("com.tweetapp.Model.Model.UserDetails", b =>
                {
                    b.Navigation("RepliesCollection");

                    b.Navigation("Tweets");

                    b.Navigation("TweetsLikedCollection");
                });
#pragma warning restore 612, 618
        }
    }
}
