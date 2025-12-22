import os

# ------------------------------------------------------------
# Skrypt do zmiany nazw plików wygenerowanych przez Unity Perception
# Zmienia:
#   step0.camera.png  -> imgXXXX.png
#   step0.frame_data.json -> imgXXXX.json
# ------------------------------------------------------------

base_dir = "synthetic_data/images"  

sequence_dirs = sorted(
    [d for d in os.listdir(base_dir) if d.startswith("sequence.")],
    key=lambda x: int(x.split(".")[1])
)

for idx, seq_dir in enumerate(sequence_dirs):
    folder_path = os.path.join(base_dir, seq_dir)
    new_id = f"img{idx+1:04d}"

    old_png = os.path.join(folder_path, "step0.camera.png")
    old_json = os.path.join(folder_path, "step0.frame_data.json")

    new_png = os.path.join(folder_path, f"{new_id}.png")
    new_json = os.path.join(folder_path, f"{new_id}.json")

    if os.path.exists(old_png):
        os.rename(old_png, new_png)
    else:
        print(f"Nie znaleziono pliku PNG w: {folder_path}")

    if os.path.exists(old_json):
        os.rename(old_json, new_json)
    else:
        print(f"Nie znaleziono pliku JSON w: {folder_path}")

print("Zmieniono nazwy wszystkich plików.")
