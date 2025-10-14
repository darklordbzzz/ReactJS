import requests
import base64
from config import SPOTIFY_CLIENT_ID, SPOTIFY_CLIENT_SECRET
from app.models.track import Track

class SpotifyAPI:
    def __init__(self):
        self.client_id = SPOTIFY_CLIENT_ID
        self.client_secret = SPOTIFY_CLIENT_SECRET
        self.token_url = "https://accounts.spotify.com/api/token"
        self.api_url = "https://api.spotify.com/v1/"
        self.access_token = self._get_access_token()

    def _get_access_token(self):
        auth_string = f"{self.client_id}:{self.client_secret}"
        auth_bytes = auth_string.encode("utf-8")
        auth_base64 = str(base64.b64encode(auth_bytes), "utf-8")

        headers = {
            "Authorization": f"Basic {auth_base64}",
            "Content-Type": "application/x-www-form-urlencoded",
        }
        data = {"grant_type": "client_credentials"}

        response = requests.post(self.token_url, headers=headers, data=data)

        if response.status_code == 200:
            return response.json().get("access_token")
        else:
            return None

    def get_track(self, track_id):
        if not self.access_token:
            return None

        headers = {"Authorization": f"Bearer {self.access_token}"}
        response = requests.get(f"{self.api_url}tracks/{track_id}", headers=headers)

        if response.status_code == 200:
            return Track(response.json())
        else:
            return None