cd /Users/shivchauhan/Work/University/Semester-8/CAPGEMINI-TRAINING

git add .

if ! git diff --cached --quiet; then
  git commit -m "Daily auto commit $(date '+%Y-%m-%d %H:%M')"
  git push origin main
fi