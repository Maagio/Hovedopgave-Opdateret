import { Component, OnInit } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Router} from "@angular/router";
import {Storage} from "@ionic/storage";

@Component({
  selector: 'app-section',
  templateUrl: './section.page.html',
  styleUrls: ['./section.page.scss'],
})
export class SectionPage implements OnInit {

  public section: Section;
  public images: Picture[] = [];
  public pictureIdArray: number[] = [];
  public baseUrl = "http://localhost:5000/";
  public jsonData = [];
  public imageString: string = null;

  constructor(private http: HttpClient, private router: Router, private storage: Storage) { }

  async ngOnInit()
  {
      await this.storage.get("section").then((val) => {
          this.section = val;
      });
      await this.GetImages();
  }

  async OnFileSelected(event)
  {
      let image = await this.ImageTest(event.target.files[0]);
      let splitedImageBase64 = await (<string>image).split(",");
      this.imageString = splitedImageBase64[1];
  }

  AddToArray(e, pictureId: number)
  {
      if (e.target.checked === true)
      {
          let index = this.pictureIdArray.indexOf(pictureId);

          if (index > -1)
              this.pictureIdArray.splice(index, 1);

      }
      else
      {
          this.pictureIdArray.push(pictureId);
      }
  }
  async ImageTest(image: File)
  {
      let reader = new FileReader();

      return new Promise((resolve, reject) =>
      {
          reader.onload = function () {
              resolve(reader.result);
          };
          reader.onerror = function (error) {
              reject(error);
          };
          reader.readAsDataURL(image);
      });
  }

  async UploadImage()
  {
      let url = this.baseUrl + "api/Section/SaveImage";
      let couldSave = false as boolean;
      this.jsonData.push(this.section.sectionId, this.imageString);


      await this.http.post<boolean>(url, JSON.stringify(this.jsonData),
          {headers: new HttpHeaders().set("Content-Type", "application/json")})
          .toPromise().then(result => {
              couldSave = result
          }, error => console.error(error));

      if ( couldSave === true)
      {
          window.location.reload();
      }
      this.jsonData = [];
  }

  async ComparePictures()
  {
      let url = this.baseUrl + "api/Section/ComparePictures";
      let couldUpload = false as boolean;
      if (this.pictureIdArray.length === 2)
      {
          this.jsonData.push(this.pictureIdArray[0], this.pictureIdArray[1]);

          await this.http.post<boolean>(url, JSON.stringify(this.jsonData),
              {headers: new HttpHeaders().set("Content-Type", "application/json")})
              .toPromise().then(result => {
                  couldUpload = result
              }, error => console.error(error));

          this.jsonData = [];

          window.location.reload();
      }

      else
          window.alert("Vælg 2 billeder for at sammenligne");
  }
  async GetImages()
  {
      let url = this.baseUrl + "api/Section/GetImages";
      this.jsonData.push(this.section.sectionId);

      await this.http.post<Picture[]>(url, JSON.stringify(this.jsonData),
          {headers: new HttpHeaders().set("Content-Type", "application/json")})
        .toPromise().then(result => {
              this.images = result
          }, error => console.error(error));

      this.jsonData = [];
  }
  async DeleteImage(pictureId: number)
  {
      let url = this.baseUrl + "api/Section/DeleteImage";
      let couldDelete = false as boolean;
      this.jsonData.push(pictureId);

      let deleteUser = window.confirm("Er du sikker på at slette dette billede?");
      if (deleteUser)
      {
          await this.http.post<boolean>(url, JSON.stringify(this.jsonData),
              {headers: new HttpHeaders().set("Content-Type", "application/json")})
              .toPromise().then(result => {
                  couldDelete = result
              }, error => console.error(error));
      }
      this.jsonData = [];

      if (couldDelete === true)
          window.location.reload();
      else
      {
          // Display error message
      }
  }
  async DeleteSection()
  {
      let url = this.baseUrl + "api/Section/DeleteSection";
      let couldDelete = false as boolean;
      this.jsonData.push(this.section.sectionId);

      let deleteUser = window.confirm("Er du sikker på at slette denne sektion?");
      if (deleteUser)
      {
          await this.http.post<boolean>(url, JSON.stringify(this.jsonData),
              {headers: new HttpHeaders().set("Content-Type", "application/json")})
              .toPromise().then(result => {
                  couldDelete = result
              }, error => console.error(error));
      }
      this.jsonData = [];

      if (couldDelete === true)
      {
          await this.router.navigate(["section-overview"]);
          window.location.reload();
      }
  }
  async GoBack()
  {
      await this.router.navigate(["section-overview"]);
      window.location.reload();
  }
}
interface Section {
    sectionId: number;
    name: string;
    sectionCreationDate: string;
    userId: number;
}

interface Picture
{
    pictureId: number,
    imageString: string,
    pictureTakenDate: string,
    userTaken: boolean,
    sectionId: number
}
