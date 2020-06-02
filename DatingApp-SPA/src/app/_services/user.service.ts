import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { User } from '../_models/user';
import {  Observable } from 'rxjs';

// const httpOptions = {

//   headers: new HttpHeaders({

//     Authorization: 'Bearer ' + localStorage.getItem('token')
//   })
// }; headers can be passed from here as well 

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiUrl;


constructor(private http: HttpClient) { }

getUsers(): Observable<User[]> {

  return this.http.get<User[]>(this.baseUrl + 'user/getusers/');

}

getUser(id): Observable<User> {

  return this.http.get<User>(this.baseUrl + 'user/getuser/' + id);
}

updateUser(id: number, user: User)
{
  return this.http.put(this.baseUrl + 'user/' + id, user);
}

}
