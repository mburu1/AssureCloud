import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full'
  },
  {
    path: 'dashboard',
    loadComponent: () => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent)
  },
  {
    path: 'organizations',
    loadComponent: () => import('./features/organizations/organizations.component').then(m => m.OrganizationsComponent),
    canActivate: [() => true]
  },
  {
    path: 'organizations/:id',
    loadComponent: () => import('./features/organizations/organization-detail.component').then(m => m.OrganizationDetailComponent)
  },
  {
    path: 'programs',
    loadComponent: () => import('./features/programs/programs.component').then(m => m.ProgramsComponent)
  },
  {
    path: 'programs/:id',
    loadComponent: () => import('./features/programs/program-detail.component').then(m => m.ProgramDetailComponent)
  },
  {
    path: 'assessments',
    loadComponent: () => import('./features/assessments/assessments.component').then(m => m.AssessmentsComponent)
  },
  {
    path: 'assessments/:id',
    loadComponent: () => import('./features/assessments/assessment-detail.component').then(m => m.AssessmentDetailComponent)
  },
  {
    path: 'audits',
    loadComponent: () => import('./features/audits/audits.component').then(m => m.AuditsComponent)
  },
  {
    path: 'audits/:id',
    loadComponent: () => import('./features/audits/audit-detail.component').then(m => m.AuditDetailComponent)
  },
  {
    path: 'certifications',
    loadComponent: () => import('./features/certifications/certifications.component').then(m => m.CertificationsComponent)
  },
  {
    path: 'certifications/:id',
    loadComponent: () => import('./features/certifications/certification-detail.component').then(m => m.CertificationDetailComponent)
  },
  {
    path: 'evidence',
    loadComponent: () => import('./features/evidence/evidence.component').then(m => m.EvidenceComponent)
  },
  {
    path: 'evidence/:id',
    loadComponent: () => import('./features/evidence/evidence-detail.component').then(m => m.EvidenceDetailComponent)
  },
  {
    path: 'reports',
    loadComponent: () => import('./features/reports/reports.component').then(m => m.ReportsComponent)
  },
  {
    path: 'reports/:id',
    loadComponent: () => import('./features/reports/report-detail.component').then(m => m.ReportDetailComponent)
  },
  {
    path: 'administration',
    loadComponent: () => import('./features/administration/administration.component').then(m => m.AdministrationComponent)
  },
  {
    path: 'login',
    loadComponent: () => import('./features/administration/login.component').then(m => m.LoginComponent)
  },
  {
    path: '**',
    redirectTo: 'dashboard'
  }
];
