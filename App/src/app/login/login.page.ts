import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { HTTP } from "@ionic-native/http/ngx";
import { Router } from "@angular/router";
import { Storage } from "@ionic/storage";

@Component({
  selector: 'app-login',
  templateUrl: './login.page.html',
  styleUrls: ['./login.page.scss'],
})
export class LoginPage implements OnInit {

  public baseUrl = "http://localhost:5000/";
  //public baseUrl = "http://192.168.43.161:5000/";
  public jsonData = [];
  constructor(private http: HttpClient, private httpMobile: HTTP, private router: Router, private storage: Storage) {}

  ngOnInit() {}

  async Login(email: string, password: string)
  {

    let url = this.baseUrl + "api/Home/Login";
    let userId = 0 as number;
    this.jsonData.push(email, password);

    /*this.httpMobile.setDataSerializer("json");
    await this.httpMobile.post(url, JSON.stringify(this.jsonData), {})
        .then(result => {
          console.log(result);
          //userId = result.data;
        })
        .catch(error => {
          console.log(error);
        });*/

    await this.http.post<number>(url, JSON.stringify(this.jsonData),
        {headers: new HttpHeaders().set("Content-Type", "application/json")})
        .toPromise().then(result => {
          userId = result
        }, error => console.error(error));

    if(userId != 0)
    {
        await this.storage.set("userId", userId);
        await this.router.navigate(["section-overview"], {state: {data: userId}});
    }
    else {
      //window.location.reload();
        // Error message
    }
    this.jsonData = [];
  }

  CreateUser()
  {
    this.router.navigate(["create-user"], {state: {data: 4}})
  }

}
