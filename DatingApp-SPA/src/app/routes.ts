import { Routes } from '@angular/router';
import { MesssagesComponent } from './messsages/messsages.component';
import { ListsComponent } from './lists/lists.component';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { AuthGuard } from './_gaurds/auth.guard';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberDetailResolver } from './_resolver/member-detail.resolver';
import { MemberListResolver } from './_resolver/member-list.resolver';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberEditResolver } from './_resolver/member-edit.resolver';
import { PreventUnsavedChanges } from './_gaurds/prevent-unsaved-changes.gaurd';

export const apppRoutes: Routes = [
  { path: 'home', component: HomeComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      { path: 'members', component: MemberListComponent , resolve: {users: MemberListResolver } },
      { path: 'members/:id', component: MemberDetailComponent , resolve: {user: MemberDetailResolver} },
      { path: 'member/edit', component: MemberEditComponent , resolve: {user: MemberEditResolver},
        canDeactivate : [PreventUnsavedChanges]},

      { path: 'messages', component: MesssagesComponent },
      { path: 'lists', component: ListsComponent },
    ],
  },
  { path: '**', redirectTo: 'home', pathMatch: 'full' },
];
