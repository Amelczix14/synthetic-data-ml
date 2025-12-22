import os
import shutil
import random
from glob import glob

# ------------------------------------------------------------
# Skrypt do podziału danych syntetycznych na zbiory treningowe,
# walidacyjne i testowe zgodnie z formatem YOLO.
# ------------------------------------------------------------

SOURCE_DIR = "synthetic_data/images"
DEST_DIR = "synthetic_data/synthetic_set"

train_ratio = 0.8
val_ratio = 0.1
test_ratio = 0.1

sequences = sorted(glob(os.path.join(SOURCE_DIR, "sequence.*")))

random.seed(42)
random.shuffle(sequences)

total = len(sequences)
train_count = int(train_ratio * total)
val_count = int(val_ratio * total)

splits = {
    "train": sequences[:train_count],
    "val": sequences[train_count:train_count + val_count],
    "test": sequences[train_count + val_count:]
}

for split in ["train", "val", "test"]:
    os.makedirs(os.path.join(DEST_DIR, "images", split), exist_ok=True)
    os.makedirs(os.path.join(DEST_DIR, "labels", split), exist_ok=True)

for split, seqs in splits.items():
    for seq_path in seqs:
        for file in os.listdir(seq_path):
            if file.endswith(".png") or file.endswith(".txt"):
                src_file = os.path.join(seq_path, file)
                if file.endswith(".png"):
                    dst_dir = os.path.join(DEST_DIR, "images", split)
                else:
                    dst_dir = os.path.join(DEST_DIR, "labels", split)
                dst_file = os.path.join(dst_dir, file)
                shutil.copy(src_file, dst_file)

print("Pliki przeniesione do struktury YOLO.")
