#!/bin/bash

# Navigate to your repository
cd /Users/shivchauhan/Work/University/Semester-8/CAPGEMINI-TRAINING

# Pull latest changes
git pull origin main

# Add all changes
git add .

# Commit with timestamp
git commit -m "Auto commit on $(date)"

# Push changes
git push origin main