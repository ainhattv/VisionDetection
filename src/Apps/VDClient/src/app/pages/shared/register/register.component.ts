import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';

import { AuthenticationService } from '../../../core/services/authentication.service';
import { UserRegister } from '../../../shared/models/UserRegister.model';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  public registerForm: FormGroup;
  public loading = false;
  public submitted = false;
  public submitDisabled = true;
  public error = '';

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authenticationService: AuthenticationService
  ) { }

  ngOnInit() {
    this.registerForm = this.formBuilder.group({
      email: ['', Validators.required],
      passWord: ['', Validators.required],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required]
    });

    // reset login status
    this.authenticationService.logout();
  }

  // convenience getter for easy access to form fields
  get f() { return this.registerForm.controls; }

  public inputChanges() {
    if (this.f.email.value !== '' &&
      this.f.passWord.value !== '' &&
      this.f.firstName.value !== '' &&
      this.f.lastName.value !== ''
    ) {
      this.submitDisabled = false;
    } else {
      this.submitDisabled = true;
    }
  }

  public onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.registerForm.invalid) {
      return;
    }

    const registerModel: UserRegister = {
      email: this.f.email.value,
      passWord: this.f.passWord.value,
      firstName: this.f.firstName.value,
      lastName: this.f.lastName.value
    };

    this.loading = true;
    this.authenticationService.register(registerModel)
      .pipe(first())
      .subscribe(
        data => {
          if (data.succeeded) {
            this.router.navigate(['login']);
          } else {
            this.error = data.errors[0].description;
          }
          this.loading = false;
        },
        error => {
          this.error = error;
          this.loading = false;
        });
  }

}
