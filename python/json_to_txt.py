import json
import os

# ------------------------------------------------------------
# Skrypt do konwersji adnotacji Unity Perception (JSON)
# do formatu YOLO (TXT)
# ------------------------------------------------------------

class_map = {
    "banana": 0,
    "apple": 1,
    "pear": 2,
    "tomato": 3
}

def convert_json_to_yolo(json_path):
    with open(json_path, 'r') as f:
        data = json.load(f)

    width = data['captures'][0]['dimension'][0]
    height = data['captures'][0]['dimension'][1]

    annotations = data['captures'][0]['annotations']
    bbox_data = None
    for ann in annotations:
        if ann['@type'].endswith('BoundingBox2DAnnotation'):
            bbox_data = ann['values']
            break

    if bbox_data is None:
        return []

    yolo_lines = []
    for obj in bbox_data:
        label = obj['labelName']
        if label not in class_map:
            continue
        class_id = class_map[label]

        x_min, y_min = obj['origin']
        w, h = obj['dimension']

        x_center = x_min + w / 2
        y_center = y_min + h / 2

        x_center_norm = x_center / width
        y_center_norm = y_center / height
        w_norm = w / width
        h_norm = h / height

        line = f"{class_id} {x_center_norm:.6f} {y_center_norm:.6f} {w_norm:.6f} {h_norm:.6f}"
        yolo_lines.append(line)
    return yolo_lines

def process_all_sequences(base_folder, num_sequences=4000):
    for i in range(1, num_sequences + 1):
        folder = os.path.join(base_folder, f"sequence.{i}")
        if not os.path.isdir(folder):
            print(f"Folder nie istnieje: {folder}")
            continue

        for filename in os.listdir(folder):
            if filename.endswith('.json'):
                json_path = os.path.join(folder, filename)
                yolo_lines = convert_json_to_yolo(json_path)

                if not yolo_lines:
                    print(f"Brak ramek ograniczających w pliku {json_path}")
                    continue

                txt_filename = os.path.splitext(filename)[0] + '.txt'
                txt_path = os.path.join(folder, txt_filename)

                with open(txt_path, 'w') as f:
                    f.write('\n'.join(yolo_lines))
                print(f"Zapisano: {txt_path}")

process_all_sequences("synthetic_data/images")
