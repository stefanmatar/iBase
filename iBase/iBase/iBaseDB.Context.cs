﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace iBase
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class iBaseDB : DbContext
    {
        public iBaseDB()
            : base("name=iBaseDB")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AlbumHasArtist> AlbumHasArtists { get; set; }
        public virtual DbSet<AlbumHasTrack> AlbumHasTracks { get; set; }
        public virtual DbSet<AlbumTable> AlbumTables { get; set; }
        public virtual DbSet<ArtistTable> ArtistTables { get; set; }
        public virtual DbSet<TrackHasArtist> TrackHasArtists { get; set; }
        public virtual DbSet<TracksHasArtist> TracksHasArtists { get; set; }
        public virtual DbSet<TrackTable> TrackTables { get; set; }
    }
}