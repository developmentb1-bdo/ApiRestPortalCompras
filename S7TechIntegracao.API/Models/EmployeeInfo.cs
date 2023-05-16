﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Models
{
    public class EmployeeInfo
    {
        public int EmployeeID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Gender { get; set; }
        public string JobTitle { get; set; }
        public int? EmployeeType { get; set; }
        public int? Department { get; set; }
        public int? Branch { get; set; }
        public string WorkStreet { get; set; }
        public string WorkBlock { get; set; }
        public string WorkZipCode { get; set; }
        public string WorkCity { get; set; }
        public string WorkCounty { get; set; }
        public string WorkCountryCode { get; set; }
        public string WorkStateCode { get; set; }
        public int? Manager { get; set; }
        public int? ApplicationUserID { get; set; }
        public int? SalesPersonCode { get; set; }
        public string OfficePhone { get; set; }
        public string OfficeExtension { get; set; }
        public string MobilePhone { get; set; }
        public string Pager { get; set; }
        public string HomePhone { get; set; }
        public string Fax { get; set; }
        public string eMail { get; set; }
        public DateTime? StartDate { get; set; }
        public int? StatusCode { get; set; }
        public double? Salary { get; set; }
        public string SalaryUnit { get; set; }
        public double? EmployeeCosts { get; set; }
        public string EmployeeCostUnit { get; set; }
        public DateTime? TerminationDate { get; set; }
        public int? TreminationReason { get; set; }
        public string BankCode { get; set; }
        public string BankBranch { get; set; }
        public string BankBranchNum { get; set; }
        public string BankAccount { get; set; }
        public string HomeStreet { get; set; }
        public string HomeBlock { get; set; }
        public string HomeZipCode { get; set; }
        public string HomeCity { get; set; }
        public string HomeCounty { get; set; }
        public string HomeCountry { get; set; }
        public string HomeState { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string CountryOfBirth { get; set; }
        public string MartialStatus { get; set; }
        public int? NumOfChildren { get; set; }
        public string IdNumber { get; set; }
        public string CitizenshipCountryCode { get; set; }
        public string PassportNumber { get; set; }
        public DateTime? PassportExpirationDate { get; set; }
        public string Picture { get; set; }
        public string Remarks { get; set; }
        public string SalaryCurrency { get; set; }
        public string EmployeeCostsCurrency { get; set; }
        public string WorkBuildingFloorRoom { get; set; }
        public string HomeBuildingFloorRoom { get; set; }
        public int? Position { get; set; }
        public string CostCenterCode { get; set; }
        public string CompanyNumber { get; set; }
        public int? VacationPreviousYear { get; set; }
        public int? VacationCurrentYear { get; set; }
        public string MunicipalityKey { get; set; }
        public string TaxClass { get; set; }
        public string IncomeTaxLiability { get; set; }
        public string Religion { get; set; }
        public string PartnerReligion { get; set; }
        public double? ExemptionAmount { get; set; }
        public string ExemptionUnit { get; set; }
        public string ExemptionCurrency { get; set; }
        public double? AdditionalAmount { get; set; }
        public string AdditionalUnit { get; set; }
        public string AdditionalCurrency { get; set; }
        public string TaxOfficeName { get; set; }
        public string TaxOfficeNumber { get; set; }
        public string HealthInsuranceName { get; set; }
        public string HealthInsuranceCode { get; set; }
        public string HealthInsuranceType { get; set; }
        public string SocialInsuranceNumber { get; set; }
        public string ProfessionStatus { get; set; }
        public string EducationStatus { get; set; }
        public string PersonGroup { get; set; }
        public string JobTitleCode { get; set; }
        public string BankCodeForDATEV { get; set; }
        public string DeviatingBankAccountOwner { get; set; }
        public string SpouseFirstName { get; set; }
        public string SpouseSurname { get; set; }
        public string ExternalEmployeeNumber { get; set; }
        public string BirthPlace { get; set; }
        public string PaymentMethod { get; set; }
        public int? STDCode { get; set; }
        public string CPF { get; set; }
        public string CRCNumber { get; set; }
        public string AccountantResponsible { get; set; }
        public string LegalRepresentative { get; set; }
        public string DIRFResponsible { get; set; }
        public string CRCState { get; set; }
        public string Active { get; set; }
        public string IDType { get; set; }
        public int? BPLID { get; set; }
        public DateTime? PassportIssueDate { get; set; }
        public string PassportIssuer { get; set; }
        public string QualificationCode { get; set; }
        public string PRWebAccess { get; set; }
        public string PreviousPRWebAccess { get; set; }
        public string WorkStreetNumber { get; set; }
        public string HomeStreetNumber { get; set; }
        public string LinkedVendor { get; set; }
        public DateTime? CreateDate { get; set; }
        public TimeSpan? CreateTime { get; set; }
        public DateTime? UpdateDate { get; set; }
        public TimeSpan? UpdateTime { get; set; }
        public string U_S7T_AreaFuncional { get; set; }
        public string U_S7T_CentroCusto { get; set; }
        public string U_S7T_GestorArea { get; set; }
        public string U_S7T_DiretorArea { get; set; }
        public string U_S7T_TipoUsuario { get; set; }
        public string U_S7T_CodUsuario { get; set; }
        public string U_S7T_SenhaPortal { get; set; }
    }
}