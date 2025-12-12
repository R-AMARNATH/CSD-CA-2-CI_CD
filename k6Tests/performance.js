import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
  stages: [
    { duration: "1m", target: 20 },   // ramp up
    { duration: "1m", target: 20 },   // hold
    { duration: "1m", target: 0 },    // ramp down
  ],
  thresholds: {
    http_req_duration: ["p(95) < 300"],  // 95% under 300ms
    http_req_failed: ["rate==0"],        // no failed HTTP requests
  },
  
  discardResponseBodies: false,
};

const BASE_URL = __ENV.BASE_URL || "http://localhost:5000";
const pagePath = "/"; 

export default function () {
  // 1) GET the page
  const resGet = http.get(`${BASE_URL}${pagePath}`);

  check(resGet, {
    "GET returned 200": (r) => r.status === 200,
  });

  // 2) Extract the anti-forgery token from the HTML
  const tokenMatch = resGet.body.match(/name="__RequestVerificationToken".+?value="([^"]+)"/);
  const token = tokenMatch && tokenMatch[1];

  check(token, {
    "anti-forgery token found": () => token !== undefined && token !== null,
  });

  // 3) Build POST payload (token + form fields)
  const payload = {
    "__RequestVerificationToken": token,
    "BP.Systolic": 120,
    "BP.Diastolic": 80,
  };

  const resPost = http.post(
    `${BASE_URL}${pagePath}`,
    payload,
    {
      headers: {
        "Content-Type": "application/x-www-form-urlencoded",
      },
    }
  );

  // Optional debug: uncomment this to see actual status codes
  // console.log(`GET status = ${resGet.status}, POST status = ${resPost.status}`);

  check(resPost, {
    "POST returned OK or redirect": (r) => r.status === 200 || r.status === 302,
  });

  sleep(3); // "think time"
}
