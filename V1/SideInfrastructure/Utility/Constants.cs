﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SideAdmin.Utility
{
    public class ConfigurationConstants
    {
        public const string DocserverUrl = "DocServer";
        public const string ThumbnailUrl = "Thumbnail";
        public const string DomainUrl = "Domain";
    }
    public static class LoginConstants
    {
        public const string LoginSession = "loginSession";
    }

    
    public enum LookupTypes
    {
        Section = 3, Document = 4, IsotopeSige = 5, IsotopDataCatagory =6, Attributes= 14
    }
    /// <summary>
    /// Defines modules of this projects
    /// </summary>
    public enum SectionTypes
    {
        HomePageCaraosel = 7, Project = 8
    }

    public enum IsotopeSigeType { 
        Big = 9, Small, wide, tall
    }

    public enum IsotopDataCatagoryType
    {
        Residential = 13
    }

    public enum SectionAttributeTypes
    {
        ProjectCheckList = 15
    }

    public enum ComponentStatus
    {
        Active =1, InActive = 0
    }
}