import {Component, OnInit} from '@angular/core';
import {HttpClient, HttpHeaders, HttpParams} from "@angular/common/http";
import {Router} from "@angular/router";
import {Storage} from "@ionic/storage";
import {Base64} from "@ionic-native/base64/ngx";

@Component({
  selector: 'app-section-overview',
  templateUrl: './section-overview.page.html',
  styleUrls: ['./section-overview.page.scss'],
})
export class SectionOverviewPage implements OnInit {

    public sections: Section[] = [];
    public test = "hej";
    public userId = 0;
    public baseUrl = "http://localhost:5000/";
    public jsonData = [];
    public hidden = false;

    constructor(private http: HttpClient, private router: Router, private storage: Storage, private  base64: Base64) {}
    async ngOnInit()
    {
        await this.storage.get("userId").then((val) => {
            this.userId = val;
        });

        await this.GetSections().then(() =>
            this.hidden = true);

        for(let section of this.sections) {
            await this.GetImages(section);
        };
    }

    async GetImages(section: Section)
    {
        let url = this.baseUrl + "api/Section/GetImages";
        this.jsonData.push(section.sectionId);

        await this.http.post<Picture[]>(url, JSON.stringify(this.jsonData),
            {headers: new HttpHeaders().set("Content-Type", "application/json")})
            .toPromise().then(result => {
                section.images = result
            }, error => console.error(error));

        this.jsonData = [];
    }

  async CreateSection(name: string)
  {
    let url = this.baseUrl + "api/Section/CreateSection";
    let userCheck = false as boolean;
    this.jsonData.push(this.userId, name);

    if (name != "") {
        await this.http.post<boolean>(url, JSON.stringify(this.jsonData),
            {headers: new HttpHeaders().set("Content-Type", "application/json")})
            .toPromise().then(result => {
                userCheck = result
            }, error => console.error(error));
    }

    this.jsonData = [];
    if (userCheck === true)
      window.location.reload();
  }

  async GetSections()
  {
      let url = this.baseUrl + "api/Section/GetSections";
      this.jsonData.push(this.userId);

      /*//let params = new HttpParams().set("userId", "test");
      await this.http.get<Section[]>(url,{headers: new HttpHeaders().set("Content-Type", "application/json"), params: {userId: "test"}})
          .toPromise().then(result => {
              this.sections = result as Section[]
          }, error => console.error(error));*/

      await this.http.post<Section[]>(url, JSON.stringify(this.jsonData),
          {headers: new HttpHeaders().set("Content-Type", "application/json")})
          .toPromise().then(result => {
              this.sections = result as Section[]
          }, error => console.error(error));
      //await console.log(this.sections);

      this.jsonData = [];
  }
  async GoToSection(section: Section)
  {
      await this.storage.set("section", section);
      this.router.navigate(["section"])
  }
  async Logout()
  {
    await this.router.navigate(["login"]);
    window.location.reload();
  }
}
interface Section {
    sectionId: number;
    name: string;
    creationDate: string;
    userId: number;
    images: Picture[];
}

interface Picture
{
    pictureId: number,
    imageString: string,
    pictureTakenDate: string,
    userTaken: boolean,
    sectionId: number
}
