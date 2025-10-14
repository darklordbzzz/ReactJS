from app import app
from flask import render_template, jsonify

@app.route('/')
@app.route('/index')
def index():
    return render_template('index.html')

@app.route('/api/data')
def api_data():
    return jsonify({'message': 'Hello from the API!'})
